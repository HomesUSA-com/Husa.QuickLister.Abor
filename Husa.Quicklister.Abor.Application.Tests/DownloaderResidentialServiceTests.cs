namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.CompanyServicesManager.Api.Contracts.Request;
    using Response = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class DownloaderResidentialServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<IAgentRepository> agentRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IDownloaderCtxClient> downloaderCtxClient = new();
        private readonly Mock<ILogger<ResidentialService>> logger = new();

        public DownloaderResidentialServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new ResidentialService(
                this.listingSaleRepository.Object,
                this.agentRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.downloaderCtxClient.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public IResidentialService Sut { get; set; }

        [Fact]
        public async Task ProcessDataFromDownloaderThrowsNotFoundExceptionWhenCompanyIsNotFoundAsync()
        {
            // Arrange
            const string entityKey = "some-entity-key";
            const bool processFullListing = true;
            var residentialResponse = TestModelProvider.GetResidentialResponse();

            this.downloaderCtxClient.Setup(s => s.Residential.GetByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(residentialResponse)
            .Verifiable();

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(Array.Empty<Response.Company>(), total: 0))
            .Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<Response.Company>>(() => this.Sut.ProcessData(entityKey, processFullListing));

            // Assert
            this.serviceSubscriptionClient.Verify();
            Assert.Equal(residentialResponse.ListingMessage.OwnerName, notFoundException.Id);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndListingNotExistsAndSuccess_ListingSaleIsCreatesOnDatabase()
        {
            // Arrange
            const string agentMarketUniqueId = "some-agent-unique-id";
            const string entityKey = "some-entity-key";
            const bool processFullListing = true;
            var residentialResponse = TestModelProvider.GetResidentialResponse();
            var agentId = Guid.NewGuid();
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            residentialResponse.OtherMessage.AgentSell = agentMarketUniqueId;
            residentialResponse.ListingMessage.MlsId = mlsNumber;
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();

            fullListingSaleDto.MlsNumber = mlsNumber;

            this.downloaderCtxClient.Setup(s => s.Residential.GetByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(residentialResponse)
            .Verifiable();

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(TestModelProvider.GetCompanyInfo(), TestModelProvider.GetCompanyInfo().Count()))
                .Verifiable();

            this.listingSaleRepository.Setup(
                r => r.GetListingByLocationAsync(
                    It.Is<string>(x => x == mlsNumber),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var agent = new Mock<Agent>();
            agent.SetupGet(a => a.Id).Returns(agentId);
            this.agentRepository
                .Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(x => x == agentMarketUniqueId)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessData(entityKey, processFullListing);

            // Assert
            this.serviceSubscriptionClient.Verify();
            this.listingSaleRepository.Verify();
            this.listingSaleRepository.Verify(x => x.Attach(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsynsAndListingExistsAndSuccess_ListingSaleIsUpdatedOnDatabase()
        {
            // Arrange
            const string entityKey = "some-entity-key";
            const bool processFullListing = true;
            const string agentMarketUniqueId = "some-agent-unique-id";
            var agentId = Guid.NewGuid();
            var listingSaleId = Guid.NewGuid();
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var residentialResponse = TestModelProvider.GetResidentialResponse();

            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            residentialResponse.OtherMessage.AgentSell = agentMarketUniqueId;
            residentialResponse.ListingMessage.MlsId = mlsNumber;
            fullListingSaleDto.MlsNumber = mlsNumber;

            this.downloaderCtxClient.Setup(s => s.Residential.GetByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(residentialResponse)
            .Verifiable();

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(TestModelProvider.GetCompanyInfo(), TestModelProvider.GetCompanyInfo().Count()));

            this.listingSaleRepository.Setup(
                r => r.GetListingByLocationAsync(
                    It.Is<string>(x => x == mlsNumber),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(TestModelProvider.GetListingSaleEntity(listingSaleId))
                .Verifiable();

            var agent = new Mock<Agent>();
            agent.SetupGet(a => a.Id).Returns(agentId);
            this.agentRepository
                .Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(x => x == agentMarketUniqueId)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessData(entityKey, processFullListing);

            // Assert
            this.serviceSubscriptionClient.Verify();
            this.listingSaleRepository.Verify();
            this.listingSaleRepository.Verify(x => x.Attach(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }
    }
}
