namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.OpenHouse;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class OpenHouseService : IOpenHouseService
    {
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ILogger<OpenHouseService> logger;

        public OpenHouseService(
            IListingSaleRepository listingSaleRepository,
            IMapper mapper,
            ILogger<OpenHouseService> logger)
        {
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessData(string mlsNumber, OpenHouseDto openHouseDto)
        {
            var saleListing = await this.listingSaleRepository.GetListingByMlsNumber(mlsNumber) ?? throw new NotFoundException<SaleListing>(mlsNumber);
            this.logger.LogInformation("Starting to process open houses from downloader for listing sale with Mls number {mlsNumber}", mlsNumber);
            var openHouses = this.mapper.Map<SaleListingOpenHouse>(openHouseDto);
            if (!saleListing.SaleProperty.ImportOpenHouseInfoFromMarket(openHouses))
            {
                this.logger.LogInformation("The open house information for the listing with mls number {mlsNumber} was not imported", mlsNumber);
                return;
            }

            await this.listingSaleRepository.SaveChangesAsync(saleListing);
        }
    }
}
