namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.CTX.Api.Contracts.Response.Residential;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Extensions.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.ServiceBus.Contracts;
    using Microsoft.Extensions.Logging;
    using QLExtEnum = Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingService : IListingService
    {
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ILotListingRepository lotListingRepository;
        private readonly ILogger<ListingService> logger;
        private readonly IAgentRepository agentRepository;
        private readonly IDownloaderCtxClient downloaderClient;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly ISaleListingMigrationService listingMigrationService;
        private readonly IMediaService mediaService;

        public ListingService(
            IListingSaleRepository listingSaleRepository,
            ILotListingRepository lotListingRepository,
            IAgentRepository agentRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IDownloaderCtxClient downloaderClient,
            ISaleListingMigrationService listingMigrationService,
            IMediaService mediaService,
            IMapper mapper,
            ILogger<ListingService> logger)
        {
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.lotListingRepository = lotListingRepository ?? throw new ArgumentNullException(nameof(lotListingRepository));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.downloaderClient = downloaderClient ?? throw new ArgumentNullException(nameof(downloaderClient));
            this.listingMigrationService = listingMigrationService ?? throw new ArgumentNullException(nameof(listingMigrationService));
            this.mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessData(string entityKey, bool processFullListing)
        {
            var residential = await this.downloaderClient.Residential.GetByIdAsync(entityKey);
            var companyName = residential.ListingMessage.OwnerName;
            var result = await this.serviceSubscriptionClient.Company.GetAsync(new()
            {
                CompanyName = companyName,
                MarketCode = MarketCode.Austin,
            });

            var company = result.Data.FirstOrDefault();

            if (company == null)
            {
                this.logger.LogError("Company with {companyName} not found", companyName);
                throw new NotFoundException<Company>(companyName);
            }

            switch (residential.ListingMessage.ListingType.First())
            {
                case PropertyType.Residential:
                    await this.ProcessResidentialData(residential, company.Id, processFullListing);
                    break;
                case PropertyType.Land:
                    await this.ProcessLotData(residential, company.Id);
                    break;
                default:
                    break;
            }
        }

        public async Task ProcessResidentialData(ResidentialResponse mrketresidential, Guid companyId, bool processFullListing)
        {
            var residential = this.mapper.Map<FullListingSaleDto>(mrketresidential);
            var isNewListing = false;
            var addressInfo = residential.SaleProperty.AddressInfo;
            this.logger.LogInformation("Starting to process data from downloader for listin2g by location: {listingAddress}", addressInfo.Address);
            var listingSale = await this.listingSaleRepository.GetListingByLocationAsync(
                residential.MlsNumber,
                residential.SaleProperty.AddressInfo.StreetNumber,
                residential.SaleProperty.AddressInfo.StreetName,
                residential.SaleProperty.AddressInfo.ZipCode,
                residential.SaleProperty.AddressInfo.UnitNumber);

            var listingStatusInfo = this.mapper.Map<ListingStatusFieldsInfo>(residential.StatusFieldsInfo);
            var agent = await this.agentRepository.GetAgentByMlsId(residential.SellingAgentId);
            listingStatusInfo.SetStatusChangeAgent(agent);
            var listingInfo = this.mapper.Map<ListingValueObject>(residential);
            var salePropertyInfo = this.mapper.Map<SalePropertyValueObject>(residential.SaleProperty);

            if (listingSale is null || (listingSale.LockedStatus == LockedStatus.Closed && listingSale.MlsNumber != listingInfo.MlsNumber))
            {
                isNewListing = true;
                listingSale = new SaleListing(listingInfo, listingStatusInfo, salePropertyInfo, companyId, true);
                this.listingSaleRepository.Attach(listingSale);
            }
            else if (listingSale.LockedStatus == QLExtEnum.LockedStatus.NoLocked || listingSale.LockedStatus == QLExtEnum.LockedStatus.LockedBySystem)
            {
                listingSale.ApplyMarketUpdate(
                    listingInfo,
                    listingStatusInfo,
                    salePropertyInfo,
                    companyId: companyId,
                    processFullListing);
            }

            await this.listingSaleRepository.SaveChangesAsync(listingSale);

            if (isNewListing)
            {
                await this.mediaService.ImportMediaFromMlsAsync(listingSale.Id, dispose: false);

                await this.listingMigrationService.SendMigrateListingMesage(MarketCode.Austin, new MigrateListingMessage
                {
                    CompanyId = listingSale.CompanyId,
                    MlsNumber = listingSale.MlsNumber,
                    UpdateListing = true,
                    MigrateFullListing = false,
                });
            }
        }

        public async Task ProcessLotData(ResidentialResponse mrketresidential, Guid companyId)
        {
            var listingDto = this.mapper.Map<LotListingDto>(mrketresidential);
            var lotListing = await this.lotListingRepository.GetListingByMlsNumber(listingDto.MlsNumber);

            var lotInfo = this.mapper.Map<LotValueObject>(listingDto);

            if (lotListing is null)
            {
                lotListing = new LotListing(lotInfo, companyId, true);
                this.lotListingRepository.Attach(lotListing);
            }
            else if (lotListing.LockedStatus == QLExtEnum.LockedStatus.NoLocked || lotListing.LockedStatus == QLExtEnum.LockedStatus.LockedBySystem)
            {
                lotListing.ApplyMarkeyUpdate(lotInfo);
            }

            await this.lotListingRepository.SaveChangesAsync(lotListing);
        }
    }
}
