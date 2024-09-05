namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class CommunityXmlServiceTests
    {
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<ILogger<CommunityXmlService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IXmlClient> xmlClient = new();
        private readonly ITestOutputHelper outputHelper;

        public CommunityXmlServiceTests(ITestOutputHelper outputHelper)
        {
            var xmlSubdivisionClientMock = new Mock<IXmlSubdivision>();
            this.xmlClient.SetupGet(x => x.Subdivision).Returns(xmlSubdivisionClientMock.Object);

            this.Sut = new CommunityXmlService(
                this.xmlClient.Object,
                this.communitySaleRepository.Object,
                this.userContextProvider.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object);
            this.outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        public ICommunityXmlService Sut { get; set; }

        [Fact]
        public async Task ImportSubdivisionWithoutCommunityIdSuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var subdivisionId = Guid.NewGuid();
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.EmailLeadInfo = new() { LockedEmailLeads = true };

            this.xmlClient
                .Setup(x => x.Subdivision.GetByIdAsync(It.Is<Guid>(id => id == subdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(XmlTestProvider.GetSubdivisionResponse(subdivisionId))
                .Verifiable();

            this.serviceSubscriptionClient.Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyDetail).Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, subdivisionId);

            // Assert
            this.communitySaleRepository.Verify(r => r.Attach(It.Is<CommunitySale>(c => c.CompanyId == companyId)), Times.Once);
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportSubdivisionWithCommunityIdSuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "company-name";
            var subdivisionId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId);
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.EmailLeadInfo = new() { LockedEmailLeads = true };

            var subdivisionResponse = XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId);
            this.xmlClient
                .Setup(x => x.Subdivision.GetByIdAsync(It.Is<Guid>(id => id == subdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subdivisionResponse)
                .Verifiable();
            this.communitySaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(communitySale)
            .Verifiable();

            this.serviceSubscriptionClient.Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyDetail).Verifiable();

            this.outputHelper.WriteLine("This is the school for the subdivision: {0}", subdivisionResponse.SchoolDistrict.First().School.Single(t => t.Type == SchoolType.Elementary).Name);

            // Act
            await this.Sut.ImportEntity(companyId, companyName, subdivisionId);

            // Assert
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()), Times.Once);
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportSubdivisionWithCommunityIdNotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "company-name";
            var subdivisionId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.EmailLeadInfo = new() { LockedEmailLeads = true };

            this.xmlClient
                .Setup(x => x.Subdivision.GetByIdAsync(It.Is<Guid>(id => id == subdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId))
                .Verifiable();
            this.communitySaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync((CommunitySale)null)
            .Verifiable();

            this.serviceSubscriptionClient.Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyDetail).Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.ImportEntity(companyId, companyName, subdivisionId));
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
