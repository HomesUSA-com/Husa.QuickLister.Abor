namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Application.Tests.Mocks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class OfficeServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly OfficeService officeService;
        private readonly Mock<IOfficeRepository> officeRepository;
        private readonly Mock<ILogger<OfficeService>> logger;

        public OfficeServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.officeRepository = new OfficeRepositoryMock();
            this.logger = new Mock<ILogger<OfficeService>>();
            this.officeService = new OfficeService(this.officeRepository.Object, this.fixture.Mapper, this.logger.Object);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAdnOfficeNotExists_OfficeIsCreatesSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            this.officeRepository.Setup(r => r.GetByMarketUniqueId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetOfficeEntity()).Verifiable();

            await this.officeService.ProcessDataFromDownloaderAsync(TestModelProvider.GetOfficeDto());

            this.officeRepository.Verify(r => r.GetByMarketUniqueId(It.IsAny<string>()), Times.Once);
            this.officeRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Office>()), Times.Once);
            this.officeRepository.Verify(r => r.Attach(It.IsAny<Office>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndOfficeExists_OfficeIsUpdatedSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            var officeDto = TestModelProvider.GetOfficeDto();
            officeDto.MarketUniqueId = marketUniqueId;
            this.officeRepository.Setup(r => r.GetByMarketUniqueId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetOfficeEntity()).Verifiable();

            await this.officeService.ProcessDataFromDownloaderAsync(officeDto);

            this.officeRepository.Verify(r => r.GetByMarketUniqueId(It.IsAny<string>()), Times.Once);
            this.officeRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Office>()), Times.Once);
            this.officeRepository.Verify(r => r.AddAsync(It.IsAny<Office>()), Times.Never);
        }
    }
}
