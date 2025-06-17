namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;

    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository officeRepository;
        private readonly ILogger<OfficeService> logger;
        private readonly IMapper mapper;

        public OfficeService(IOfficeRepository officeRepository, IMapper mapper, ILogger<OfficeService> logger)
        {
            this.officeRepository = officeRepository ?? throw new ArgumentNullException(nameof(officeRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessDataFromDownloaderAsync(OfficeDto officeDto)
        {
            this.logger.LogInformation("Office service is starting to process data from downloader for office with id {officeMarketUniqueId}", officeDto.MarketUniqueId);
            var office = await this.officeRepository.GetByMarketUniqueId(officeDto.MarketUniqueId);
            var officeValue = this.mapper.Map<OfficeValueObject>(officeDto);

            if (office is null)
            {
                office = new Office(officeValue);
                this.officeRepository.Attach(office);
            }
            else
            {
                office.OfficeValue ??= new OfficeValueObject();
                office.UpdateInformation(officeValue);
            }

            await this.officeRepository.SaveChangesAsync(office);
        }
    }
}
