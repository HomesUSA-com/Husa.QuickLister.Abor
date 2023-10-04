namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class DownloaderService : IDownloaderService
    {
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ILogger<DownloaderService> logger;
        private readonly IDownloaderCtxClient downloaderCtxClient;
        private readonly ISaleListingMediaService listingMediaService;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IAgentRepository agentRepository;
        private readonly ApplicationOptions options;

        public DownloaderService(
            IListingSaleRepository listingSaleRepository,
            IDownloaderCtxClient downloaderCtxClient,
            ISaleListingMediaService saleListingMediaService,
            IAgentRepository agentRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IOptions<ApplicationOptions> options,
            IMapper mapper,
            ILogger<DownloaderService> logger)
        {
            this.downloaderCtxClient = downloaderCtxClient ?? throw new ArgumentNullException(nameof(downloaderCtxClient));
            this.listingMediaService = saleListingMediaService ?? throw new ArgumentNullException(nameof(saleListingMediaService));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessDataFromDownloaderAsync(
            FullListingSaleDto listingSaleDto,
            IEnumerable<RoomDto> roomsDto,
            string agentMarketUniqueId)
        {
            var companyName = listingSaleDto.SaleProperty.SalePropertyInfo.OwnerName;
            var companies = await this.serviceSubscriptionClient.Company.GetAsync(new()
            {
                MarketCode = MarketCode.Austin,
            });

            var company = companies.Data.SingleOrDefault(c => c.Name == companyName);
            if (company == null)
            {
                this.logger.LogError("Company with {companyName} not found", companyName);
                throw new NotFoundException<Company>(companyName);
            }

            var addressInfoDto = listingSaleDto.SaleProperty.AddressInfo;
            this.logger.LogInformation("Starting to process data from downloader for listing by location: {listingAddress}", addressInfoDto.Address);
            var listingSale = await this.listingSaleRepository.GetListingByLocationAsync(
                listingSaleDto.MlsNumber,
                addressInfoDto.StreetNumber,
                addressInfoDto.StreetName,
                addressInfoDto.ZipCode);

            var listingStatusInfo = this.mapper.Map<ListingSaleStatusFieldsInfo>(listingSaleDto.StatusFieldsInfo);
            var agent = await this.agentRepository.GetAgentByMarketUniqueId(agentMarketUniqueId);
            listingStatusInfo.SetStatusChangeAgent(agent);
            var listingInfo = this.mapper.Map<ListingValueObject>(listingSaleDto);
            var salePropertyInfo = this.mapper.Map<SalePropertyValueObject>(listingSaleDto.SaleProperty);
            var roomsInfo = this.mapper.Map<IEnumerable<ListingSaleRoom>>(roomsDto);

            if (listingSale is null)
            {
                listingSale = new SaleListing(listingInfo, listingStatusInfo, salePropertyInfo, company.Id);
                listingSale.SaleProperty.AddRooms(roomsInfo);
                this.listingSaleRepository.Attach(listingSale);
            }
            else
            {
                listingSale.ApplyMarketUpdate(
                    listingInfo,
                    listingStatusInfo,
                    salePropertyInfo,
                    roomsInfo,
                    companyId: company.Id);
            }

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task ImportMediaFromMlsAsync(Guid listingId)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            this.logger.LogInformation("Listing Sale service is starting to import media from ABOR market for listing with id {listingId}", listingId);
            var mlsMedia = await this.downloaderCtxClient.MlsMedia.ImportSaleListingPhotosAsync(listingSale.MlsNumber);
            var mediaDto = this.mapper.Map<IEnumerable<ListingSaleMediaDto>>(mlsMedia);
            await this.CreateMediaAsync(listingId, mediaDto);
        }

        private async Task CreateMediaAsync(Guid listingId, IEnumerable<ListingSaleMediaDto> mlsMedia)
        {
            if (!mlsMedia.Any())
            {
                this.logger.LogInformation("No media was found in the MLS to import for listing id {listingId}", listingId);
                return;
            }

            await this.listingMediaService.Resource.DeleteAsync(listingId);
            await this.listingMediaService.Resource.BulkCreateAsync(listingId, mlsMedia, mediaLimitAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia);
        }
    }
}
