namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Xml;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using XmlListActionType = Husa.Xml.Domain.Enums.ListActionType;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingXmlServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IXmlClient> xmlClient = new();
        private readonly Mock<IServiceSubscriptionClient> companyClient = new();
        private readonly Mock<IUserContextProvider> contextProvider = new();
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ISaleListingService> listingSaleService = new();
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<ISaleListingRequestService> saleListingRequestService = new();
        private readonly Mock<IXmlMediaService> xmlMediaService = new();
        private readonly Mock<ILogger<SaleListingXmlService>> logger = new();

        public SaleListingXmlServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            var xmlListingClientMock = new Mock<IXmlListing>();
            this.xmlClient.SetupGet(x => x.Listing).Returns(xmlListingClientMock.Object);

            this.Sut = new(
                this.fixture.Mapper,
                this.xmlClient.Object,
                this.companyClient.Object,
                this.listingSaleRepository.Object,
                this.listingSaleService.Object,
                this.communitySaleRepository.Object,
                this.saleListingRequestService.Object,
                this.contextProvider.Object,
                this.xmlMediaService.Object,
                this.logger.Object);
        }

        private SaleListingXmlService Sut { get; set; }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare)]
        public async Task ProcessListingAsync_AsMlsAdministrator_Success(ListActionType actionType, XmlListActionType xmlListActionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var planId = Guid.NewGuid();

            var plan = new Plan(companyId, "plan name", "owner name");
            plan.Id = planId;

            var community = new CommunitySale(companyId, "community name", "Owner name");
            community.Id = communityId;

            this.SetupCompanyDetail();
            this.SetupMlsAdministrator(userId);
            this.SetupGetXmlListingById(xmlListingId);
            this.SetupListingQuickCreate();

            this.communitySaleRepository
              .Setup(x => x.GetById(It.Is<Guid>(x => x == xmlListingId), It.IsAny<bool>()))
              .ReturnsAsync(community)
              .Verifiable();

            // Act
            await this.Sut.ProcessListingAsync(xmlListingId, actionType);

            // Assert
            this.xmlClient.Verify(
                c => c.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(action => action.Type == xmlListActionType && action.ListOn == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            this.communitySaleRepository.Verify(
                cr => cr.IsCommunityEmployee(It.Is<Guid>(id => id == userId), It.IsAny<Guid>()),
                Times.Never);
        }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare)]
        public async Task ProcessListingAsync_AsCompanyAdmin_Success(ListActionType actionType, XmlListActionType xmlListActionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupUserEmployee(RoleEmployee.CompanyAdmin, userId, companyId);
            this.SetupCompanyDetail();
            this.SetupGetXmlListingById(xmlListingId, companyId, planId);
            this.SetupListingQuickCreate();

            var plan = new Plan(companyId, "plan name", "owner name");
            plan.Id = planId;

            var community = new CommunitySale(companyId, "community name", "Owner name");
            community.Id = communityId;
            this.communitySaleRepository
               .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
               .ReturnsAsync(community)
               .Verifiable();

            // Act
            await this.Sut.ProcessListingAsync(xmlListingId, actionType);

            // Assert
            this.xmlClient.Verify(
                c => c.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(action => action.Type == xmlListActionType && action.ListOn == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            this.communitySaleRepository.Verify(
                cr => cr.IsCommunityEmployee(It.Is<Guid>(id => id == userId), It.IsAny<Guid>()),
                Times.Never);
        }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare)]
        public async Task ProcessListingAsync_AsCommunityEmployee_Success(ListActionType actionType, XmlListActionType xmlListActionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var plan = new Plan(companyId, "plan name", "owner name");
            plan.Id = planId;

            var community = new CommunitySale(companyId, "community name", "Owner name");
            community.Id = communityId;

            this.SetupUserEmployee(RoleEmployee.SalesEmployee, userId, companyId);
            this.SetupCompanyDetail();
            this.SetupGetXmlListingById(xmlListingId, companyId, communityId, planId);
            this.SetupListingQuickCreate();

            this.communitySaleRepository
                .Setup(x => x.IsCommunityEmployee(It.Is<Guid>(id => id == userId), It.Is<Guid>(id => id == communityId)))
                .Returns(true)
                .Verifiable();

            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community)
                .Verifiable();

            // Act
            await this.Sut.ProcessListingAsync(xmlListingId, actionType);

            // Assert
            this.xmlClient.Verify(
                sl => sl.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(action => action.Type == xmlListActionType && action.ListOn == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [InlineData(ListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare)]
        public async Task ProcessListingAsync_AsCommunityEmployee_Fail(ListActionType actionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupUserEmployee(RoleEmployee.SalesEmployee, userId, companyId);
            this.SetupGetXmlListingById(xmlListingId, companyId, communityId);

            this.communitySaleRepository
                .Setup(x => x.IsCommunityEmployee(It.Is<Guid>(id => id == userId), It.Is<Guid>(id => id == communityId)))
                .Returns(false)
                .Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.Sut.ProcessListingAsync(xmlListingId, actionType));
        }

        [Fact]
        public async Task DeleteListingAsyncSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            this.SetupGetXmlListingById(xmlListingId);

            // Act
            await this.Sut.DeleteListingAsync(xmlListingId);

            // Assert
            this.xmlClient.Verify(
                sl => sl.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(action => action.Type == XmlListActionType.ListNever && action.ListOn == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ListLaterAsyncSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            this.SetupGetXmlListingById(xmlListingId);
            var listOn = DateTime.Now.AddDays(7);

            // Act
            await this.Sut.ListLaterAsync(xmlListingId, listOn);

            // Assert
            this.xmlClient.Verify(
                sl => sl.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(action => action.Type == XmlListActionType.ListLater && action.ListOn == listOn),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateListingAsyncFails()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId);
            saleListing.XmlListingId = xmlListingId;
            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.Is<Guid>(id => id == listingId)))
               .ReturnsAsync(saleListing)
               .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.UpdateListingFromXmlAsync(xmlListingId));
        }

        [Fact]
        public async Task AtuoMatchAsyncNotMatchFails()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId);
            saleListing.XmlListingId = xmlListingId;
            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.Is<Guid>(id => id == listingId)))
               .ReturnsAsync(saleListing)
               .Verifiable();

            // Act and Assert
            await this.Sut.AutoMatchListingFromXmlAsync(xmlListingId);
            this.xmlClient.Verify(
                sl => sl.Listing.ChangeMatchStatus(
                It.Is<Guid>(id => id == xmlListingId),
                It.IsAny<MatchStatusRequest>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AtuoMatchAsyncMatchSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId);
            saleListing.XmlListingId = xmlListingId;
            this.SetupGetXmlListingById(xmlListingId);
            var matchingListings = new List<SaleListing>()
            {
                saleListing,
            };

            this.listingSaleRepository
               .Setup(x => x.GetAutmaticMatchingListingsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.Is<bool>(x => !x)))
               .ReturnsAsync(matchingListings)
               .Verifiable();

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.Is<Guid>(id => id == listingId)))
               .ReturnsAsync(saleListing)
               .Verifiable();

            // Act and Assert
            await this.Sut.AutoMatchListingFromXmlAsync(xmlListingId);
            this.xmlClient.Verify(
                sl => sl.Listing.ChangeMatchStatus(
                It.Is<Guid>(id => id == xmlListingId),
                It.IsAny<MatchStatusRequest>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AtuoMatchAsyncPartialMatchSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId);
            saleListing.XmlListingId = xmlListingId;
            this.SetupGetXmlListingById(xmlListingId);
            var matchingListings = new List<SaleListing>()
            {
                saleListing,
            };
            List<SaleListing> nullMatchingListings = null;

            this.listingSaleRepository
               .Setup(x => x.GetAutmaticMatchingListingsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.Is<bool>(x => !x)))
               .ReturnsAsync(nullMatchingListings)
               .Verifiable();

            this.listingSaleRepository
               .Setup(x => x.GetAutmaticMatchingListingsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.Is<bool>(x => x)))
               .ReturnsAsync(matchingListings)
               .Verifiable();

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.Is<Guid>(id => id == listingId)))
               .ReturnsAsync(saleListing)
               .Verifiable();

            // Act and Assert
            await this.Sut.AutoMatchListingFromXmlAsync(xmlListingId);
            this.xmlClient.Verify(
                sl => sl.Listing.ChangeMatchStatus(
                It.Is<Guid>(id => id == xmlListingId),
                It.IsAny<MatchStatusRequest>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AtuoMatchAsyncAlreadyImportedFails()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId);
            saleListing.XmlListingId = xmlListingId;
            ////this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.Is<Guid>(id => id == listingId)))
               .ReturnsAsync(saleListing)
               .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.UpdateListingFromXmlAsync(xmlListingId));
        }

        [Fact]
        public async Task UpdateListingAsyncWithoutRequestSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListingMock = TestModelProvider.GetListingSaleEntityMock(listingId);
            var saleListingRequest = new Mock<SaleListingRequest>();
            saleListingMock
                .Setup(x => x.GenerateRequest(It.IsAny<Guid>()))
                .Returns(CommandSingleResult<SaleListingRequest, ValidationResult>.Success(saleListingRequest.Object))
                .Verifiable();
            var saleListing = saleListingMock.Object;
            var company = TestModelProvider.GetCompanyDetail(listingId);
            saleListing.XmlListingId = xmlListingId;
            saleListing.CompanyId = companyId;

            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.IsAny<Guid>()))
               .ReturnsAsync(saleListing)
               .Verifiable();

            this.xmlMediaService
               .Setup(x => x.ImportListingMedia(xmlListingId, false, true))
               .Verifiable();

            this.companyClient
               .Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(company)
               .Verifiable();

            // Act and Assert
            await this.Sut.UpdateListingFromXmlAsync(xmlListingId);
            this.listingSaleRepository.Verify(x => x.GetListingByXmlListingId(It.Is<Guid>(r => r == xmlListingId)), Times.Once);
        }

        [Fact]
        public async Task UpdateListingAsyncNotUpdated()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListingMock = TestModelProvider.GetListingSaleEntityMock(listingId);
            saleListingMock.Setup(x => x.MlsStatus).Returns(MarketStatuses.Closed);
            var saleListing = saleListingMock.Object;
            saleListing.XmlListingId = xmlListingId;
            saleListing.CompanyId = companyId;

            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.IsAny<Guid>()))
               .ReturnsAsync(saleListing)
               .Verifiable();

            this.xmlMediaService
               .Setup(x => x.ImportListingMedia(xmlListingId, false, true))
               .Verifiable();

            // Act and Assert
            await this.Sut.UpdateListingFromXmlAsync(xmlListingId);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }

        [Fact]
        public async Task UpdateListingAsyncWithRequestSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);
            saleListing.XmlListingId = xmlListingId;
            saleListing.MlsNumber = "123345";
            saleListing.ListPrice = 300555;
            var saleListingMock = Mock.Get(saleListing);
            var saleListingRequestMock = new Mock<SaleListingRequest>();
            saleListingMock.Setup(x => x.GenerateRequest(It.IsAny<Guid>())).Returns(CommandSingleResult<SaleListingRequest, ValidationResult>.Success(saleListingRequestMock.Object));

            var xmlListingClientMock = new Mock<IXmlListing>();
            this.xmlClient.SetupGet(x => x.Listing).Returns(xmlListingClientMock.Object);
            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.IsAny<Guid>()))
               .ReturnsAsync(saleListing)
               .Verifiable();

            this.xmlMediaService
               .Setup(x => x.ImportListingMedia(xmlListingId, false, true))
               .Verifiable();

            // Act
            await this.Sut.UpdateListingFromXmlAsync(xmlListingId);

            // Assert
            this.listingSaleRepository.Verify(x => x.GetListingByXmlListingId(It.Is<Guid>(r => r == xmlListingId)), Times.Once);
            xmlListingClientMock.Verify(
                x => x.ProcessListing(
                    It.Is<Guid>(r => r == xmlListingId),
                    It.Is<ListActionRequest>(r => r.Type == XmlListActionType.ListUpdate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateListingAsyncWithRequestGeneratedSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            this.SetupMlsAdministrator();
            var saleListing = TestModelProvider.GetListingSaleEntity(listingId, true, companyId, generateRequest: true);
            saleListing.XmlListingId = xmlListingId;
            saleListing.MlsNumber = "123345";

            this.SetupGetXmlListingById(xmlListingId);

            this.listingSaleRepository
               .Setup(x => x.GetListingByXmlListingId(It.IsAny<Guid>()))
               .ReturnsAsync(saleListing)
               .Verifiable();

            this.saleListingRequestService
               .Setup(x => x.HasOpenRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(false)
               .Verifiable();

            this.saleListingRequestService
               .Setup(x => x.GenerateRequestFromXmlAsync(It.IsAny<SaleListingRequest>(), It.IsAny<CancellationToken>()))
               .Verifiable();

            // Act and Assert
            await this.Sut.UpdateListingFromXmlAsync(xmlListingId);
            this.listingSaleRepository.Verify(x => x.GetListingByXmlListingId(It.Is<Guid>(r => r == xmlListingId)), Times.Once);
            this.saleListingRequestService.Verify(x => x.GenerateRequestFromXmlAsync(It.IsAny<SaleListingRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare)]
        public async Task ProcessListingThrowsDomainExceptionWhenMarketIsNotAustin(ListActionType listAction, XmlListActionType xmlListActionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();
            listingResource
                .Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == xmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new XmlResponse.XmlListingDetailResponse { Market = MarketCode.DFW })
                .Verifiable();

            this.xmlClient
                .SetupGet(c => c.Listing)
                .Returns(listingResource.Object)
                .Verifiable();

            // Act
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ProcessListingAsync(xmlListingId, listAction));

            // Assert
            listingResource.Verify();
            this.xmlClient.Verify();
            listingResource.Verify(
                sl => sl.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(actionRequest => actionRequest.Type == xmlListActionType),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare)]
        public async Task ProcessListingThrowsDomainExceptionWhenListingCompanyIsNotSet(ListActionType listAction, XmlListActionType xmlListActionType)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();
            listingResource
                .Setup(r => r.GetByIdAsync(It.Is<Guid>(id => id == xmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new XmlResponse.XmlListingDetailResponse { Market = MarketCode.Austin })
                .Verifiable();

            this.xmlClient
                .SetupGet(c => c.Listing)
                .Returns(listingResource.Object)
                .Verifiable();

            // Act
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ProcessListingAsync(xmlListingId, listAction));

            // Assert
            listingResource.Verify();
            this.xmlClient.Verify();
            listingResource.Verify(
                sl => sl.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(actionRequest => actionRequest.Type == xmlListActionType),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(ListActionType.ListNow, XmlListActionType.ListNow, UserRole.User)]
        [InlineData(ListActionType.ListCompare, XmlListActionType.ListCompare, UserRole.Photographer)]
        public async Task ProcessListingThrowsDomainExceptionWhenCurrentUserIsNotFromListingCompany(ListActionType listAction, XmlListActionType xmlListActionType, UserRole userRole)
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingCompanyId = Guid.NewGuid();
            this.SetupGetXmlListingById(xmlListingId, companyId: listingCompanyId);
            this.SetupUser(userRole, companyId: Guid.NewGuid());

            // Act
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ProcessListingAsync(xmlListingId, listAction));

            // Assert
            this.xmlClient.Verify(
                sl => sl.Listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(actionRequest => actionRequest.Type == xmlListActionType),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        private void SetupGetXmlListingById(Guid xmlListingId, Guid? companyId = null, Guid? communityId = null, Guid? planId = null)
        {
            this.xmlClient
                .Setup(x => x.Listing.GetByIdAsync(It.Is<Guid>(id => id == xmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new XmlResponse.XmlListingDetailResponse
                {
                    Id = xmlListingId,
                    CommunityId = communityId ?? Guid.NewGuid(),
                    CompanyId = companyId ?? Guid.NewGuid(),
                    PlanId = planId ?? Guid.NewGuid(),
                    Market = MarketCode.Austin,
                })
                .Verifiable();
        }

        private void SetupUser(UserRole role, Guid? userId = null, Guid? companyId = null)
        {
            var user = new UserContext()
            {
                Id = userId ?? Guid.NewGuid(),
                UserRole = role,
                IsMLSAdministrator = role == UserRole.MLSAdministrator,
                CompanyId = companyId,
            };
            this.contextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.contextProvider.Setup(u => u.GetCurrentUserId()).Returns(user.Id).Verifiable();
        }

        private void SetupMlsAdministrator(Guid? userId = null)
        {
            this.SetupUser(UserRole.MLSAdministrator, userId);
        }

        private void SetupUserEmployee(RoleEmployee role, Guid userId, Guid? companyId = null)
        {
            var user = new UserContext()
            {
                Id = userId,
                UserRole = UserRole.User,
                EmployeeRole = role,
                IsMLSAdministrator = false,
                CompanyId = companyId ?? Guid.NewGuid(),
            };
            this.contextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.contextProvider.Setup(u => u.GetCurrentUserId()).Returns(user.Id).Verifiable();
        }

        private void SetupCompanyDetail()
        {
            this.companyClient
                .Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CompanyDetail
                {
                    Name = "fakeName",
                })
                .Verifiable();
        }

        private void SetupListingQuickCreate()
        {
            var quickCreateResult = new Mock<SaleListing>();
            var commandResult = CommandResult<SaleListing>.Success(quickCreateResult.Object);
            this.listingSaleService
                .Setup(l => l.QuickCreateAsync(
                    It.IsAny<ListingSaleDto>(),
                    It.Is<bool>(importFromListing => !importFromListing)))
                .ReturnsAsync(commandResult);
        }
    }
}
