namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;

    public class ResidentialService : IResidentialService
    {
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ILogger<ResidentialService> logger;
        private readonly IAgentRepository agentRepository;
        private readonly IDownloaderCtxClient downloaderClient;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        public ResidentialService(
            IListingSaleRepository listingSaleRepository,
            IAgentRepository agentRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IDownloaderCtxClient downloaderClient,
            IMapper mapper,
            ILogger<ResidentialService> logger)
        {
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.downloaderClient = downloaderClient ?? throw new ArgumentNullException(nameof(downloaderClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessData(string entityKey, bool processFullListing)
        {
            var residential = await this.downloaderClient.Residential.GetByIdAsync(entityKey);
            var residentialDto = this.mapper.Map<FullListingSaleDto>(residential);
            var companyName = residentialDto.SaleProperty.SalePropertyInfo.OwnerName;
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

            var addressInfoDto = residentialDto.SaleProperty.AddressInfo;
            this.logger.LogInformation("Starting to process data from downloader for listin2g by location: {listingAddress}", addressInfoDto.Address);
            var listingSale = await this.listingSaleRepository.GetListingByLocationAsync(
                residentialDto.MlsNumber,
                residentialDto.SaleProperty.AddressInfo.StreetNumber,
                residentialDto.SaleProperty.AddressInfo.StreetName,
                residentialDto.SaleProperty.AddressInfo.ZipCode,
                residentialDto.SaleProperty.AddressInfo.UnitNumber);

            var listingStatusInfo = this.mapper.Map<ListingSaleStatusFieldsInfo>(residentialDto.StatusFieldsInfo);
            var agent = await this.agentRepository.GetAgentByMarketUniqueId(residentialDto.SellingAgentId);
            listingStatusInfo.SetStatusChangeAgent(agent);
            var listingInfo = this.mapper.Map<ListingValueObject>(residentialDto);
            var salePropertyInfo = this.mapper.Map<SalePropertyValueObject>(residentialDto.SaleProperty);

            if (listingSale is null)
            {
                listingSale = new SaleListing(listingInfo, listingStatusInfo, salePropertyInfo, company.Id, true);
                this.listingSaleRepository.Attach(listingSale);
            }
            else
            {
                listingSale.ApplyMarketUpdate(
                    listingInfo,
                    listingStatusInfo,
                    salePropertyInfo,
                    companyId: company.Id,
                    processFullListing);
            }

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }
    }
}
