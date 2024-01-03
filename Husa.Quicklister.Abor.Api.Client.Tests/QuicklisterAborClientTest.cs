namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Request;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Alert;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Xml;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using BaseFilterRequest = Husa.Quicklister.Extensions.Api.Contracts.Request.BaseFilterRequest;
    using PlanRequest = Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request;
    using Response = Husa.CompanyServicesManager.Api.Contracts.Response;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Api.Client.Tests")]
    public class QuicklisterAborClientTest
    {
        private readonly CustomWebApplicationFactory<TestStartup> customWebApplicationFactory;
        private readonly QuicklisterAborClient quicklisterAborClient;

        public QuicklisterAborClientTest(CustomWebApplicationFactory<TestStartup> customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory ?? throw new ArgumentNullException(nameof(customWebApplicationFactory));
            var client = customWebApplicationFactory.GetClient();
            var loggerFactory = this.customWebApplicationFactory.Services.GetRequiredService<ILoggerFactory>();
            this.quicklisterAborClient = new QuicklisterAborClient(loggerFactory, client);
        }

        [Fact]
        public async Task SaleListingGetAsyncSuccess()
        {
            // Arrange
            var filter = new Request.ListingSaleRequestFilter()
            {
                MlsStatus = SaleListing.ActiveAndPendingListingStatuses,
            };

            // Act
            var listings = await this.quicklisterAborClient.SaleListing.GetAsync(filter);

            // Assert
            Assert.True(listings.Count() >= 7);
        }

        [Fact]
        public async Task PlanGetAsyncSuccess()
        {
            // Arrange
            var filter = new PlanRequest.PlanRequestFilter()
            {
                SearchBy = Factory.PlanName,
            };

            // Act
            var plans = await this.quicklisterAborClient.Plan.GetAsync(filter);

            // Assert
            Assert.Single(plans);
            Assert.True(!plans.Any(x => x.Id == Factory.PlanAwaitingApprovalId));
        }

        [Fact]
        public async Task ApprovePlanSuccess()
        {
            var planId = Factory.PlanAwaitingApprovalId;

            // Act
            var task = Task.Run(() => this.quicklisterAborClient.Plan.ApprovePlan(planId));
            await task;

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task ApprovePlanNotFound()
        {
            var planId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.Plan.ApprovePlan(planId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task ApprovePlanInvalid()
        {
            var planId = Factory.PlanId;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.Plan.ApprovePlan(planId));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Theory]
        [InlineData(AlertType.NotListedInMls, 2)]
        [InlineData(AlertType.ActiveEmployees, 0)]
        [InlineData(AlertType.InadequatePublicRemarks, 1)]
        public async Task AlertGetAsyncSuccess(AlertType alertType, int expectedAlertCount)
        {
            // Arrange
            var filter = new BaseAlertFilterRequest()
            {
                Skip = 0,
                Take = 10,
            };

            // Act
            var alerts = await this.quicklisterAborClient.Alert.GetAsync(alertType, filter);

            // Assert
            Assert.Equal(expectedAlertCount, alerts.Data.Count());
        }

        [Fact]
        public async Task AlertGetCompletedHomesWithoutPhotoRequestAsyncSuccess()
        {
            // Arrange
            var filter = new BaseAlertFilterRequest()
            {
                Skip = 0,
                Take = 10,
            };

            var companyResponse = new List<Response.Company>() { new Response.Company() { Id = Factory.CompanyId } };
            var company = new DataSet<Response.Company>(companyResponse, 1);
            var companyClient = this.customWebApplicationFactory.Services.GetRequiredService<IServiceSubscriptionClient>();
            var companyResourceMock = Mock.Get(companyClient.Company);
            companyResourceMock.Setup(c => c.GetAsync(It.IsAny<CompanyRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(company);

            // Act
            var alerts = await this.quicklisterAborClient.Alert.GetAsync(AlertType.CompletedHomesWithoutPhotoRequest, filter);

            // Assert
            Assert.Single(alerts.Data);
        }

        [Fact]
        public async Task GetAlertTotalSuccess()
        {
            // Arrange
            const int expectedTotalCount = 2;
            var alerts = new List<AlertType>() { AlertType.NotListedInMls, AlertType.ActiveEmployees };

            // Act
            var total = await this.quicklisterAborClient.Alert.GetAlertTotal(alerts);

            // Assert
            Assert.Equal(expectedTotalCount, total);
        }

        [Fact]
        public async Task GetAlertTotalWithEmptyArraySuccess()
        {
            // Arrange
            const int expectedTotalCount = 0;

            // Act
            var total = await this.quicklisterAborClient.Alert.GetAlertTotal(alerts: Array.Empty<AlertType>());

            // Assert
            Assert.Equal(expectedTotalCount, total);
        }

        [Fact]
        public async Task GetPlanWithListingProjectionSuccess()
        {
            // Arrange
            var planId = Factory.PlanId;
            var listingId = Factory.ListingId;

            // Act
            var result = await this.quicklisterAborClient.Plan.GetPlanWithListingProjection(planId, listingId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPlanWithListingProjectionNotFoundPlan()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var listingId = Factory.ListingId;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.Plan.GetPlanWithListingProjection(planId, listingId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task GetPlanWithListingProjectionNotFoundListing()
        {
            // Arrange
            var planId = Factory.PlanId;
            var listingId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.Plan.GetPlanWithListingProjection(planId, listingId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task GetCommunityWithListingProjectionSuccess()
        {
            // Arrange
            var communityId = Factory.CommunityId;
            var listingId = Factory.ListingId;

            // Act
            var result = await this.quicklisterAborClient.SaleCommunity.GetCommunityWithListingProjection(communityId, listingId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCommunityWithListingProjectionNotFoundCommunity()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var listingId = Factory.ListingId;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleCommunity.GetCommunityWithListingProjection(communityId, listingId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task GetCommunityWithListingProjectionNotFoundListing()
        {
            // Arrange
            var communityId = Factory.CommunityId;
            var listingId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleCommunity.GetCommunityWithListingProjection(communityId, listingId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task CreateSaleListingAsyncError()
        {
            // Arrange
            var filter = TestModelProvider.GetListingSaleRequest(companyId: Factory.CompanyId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleListing.CreateListing(filter));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateActionTypeNotFound()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleListing.UpdateActionTypeAsync(listingId, ActionType.Comparable));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task GetReverseProspectListingNotFound()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleListing.GetReverseProspect(listingId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateListingSuccess()
        {
            // Arrange
            var listingId = Factory.ListingId;
            var listingSaleRequest = ListingTestProvider.GetListingSaleDetailRequest();

            // Act
            await this.quicklisterAborClient.SaleListing.UpdateListing(listingId, listingSaleRequest);

            // Assert
            var listing = await this.quicklisterAborClient.SaleListing.GetByIdAsync(listingId);
            Assert.Equal(listingSaleRequest.SaleProperty.FeaturesInfo.NeighborhoodAmenities, listing.SaleProperty.FeaturesInfo.NeighborhoodAmenities);
            Assert.Equal(listingSaleRequest.SaleProperty.SpacesDimensionsInfo.FullBathsTotal, listing.SaleProperty.SpacesDimensionsInfo.FullBathsTotal);
        }

        [Fact]
        public async Task GetComparisonReportAsyncSuccess()
        {
            // Arange
            var filter = new Request.ScrapedListingRequestFilter()
            {
                BuilderName = Factory.BuilderName,
            };

            // Act
            var response = await this.quicklisterAborClient.Report.GetComparisonReportAsync(filter);

            // Assert
            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task GetComparisonReportAsyncNotFound()
        {
            // Arange
            var filter = new Request.ScrapedListingRequestFilter()
            {
                BuilderName = "Unknown Builder",
            };

            // Act
            var response = await this.quicklisterAborClient.Report.GetComparisonReportAsync(filter);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetXmlListingsSuccess()
        {
            // Act
            var result = await this.quicklisterAborClient.Xml.GetListings(new XmlListingFilterRequest());

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [ClassData(typeof(DataGenerator))]
        public async Task ProcessListingAsyncSuccess(ListActionType actionType, decimal? salesPrice)
        {
            var xmlListingId = Factory.XmlListingId;

            var xmlListingDetailResponse = new XmlResponse.XmlListingDetailResponse
            {
                Market = MarketCode.Austin,
                CompanyId = Factory.CompanyId,
                CommunityId = Factory.CommunityId,
                PlanId = Factory.PlanId,
                Name = "fakeName",
                State = "TX",
                Price = 100000,
                SalesPrice = salesPrice,
                City = "Houston",
                LivingArea = "Study;FamilyRoom",
                Stories = 2,
                Baths = 2,
                Bedrooms = 2,
            };
            var xmlClient = this.customWebApplicationFactory.Services.GetRequiredService<IXmlClient>();
            var xmlResourceMock = Mock.Get(xmlClient.Listing);
            xmlResourceMock.Setup(c => c.GetByIdAsync(It.Is<Guid>(x => x == Factory.XmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(xmlListingDetailResponse);

            var companyResponse = new Response.CompanyDetail() { Id = Factory.CompanyId, Name = "fakeName" };
            var companyClient = this.customWebApplicationFactory.Services.GetRequiredService<IServiceSubscriptionClient>();
            var companyResourceMock = Mock.Get(companyClient.Company);
            companyResourceMock.Setup(c => c.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyResponse);

            // Act
            var task = Task.Run(() => this.quicklisterAborClient.Xml.ProcessListingAsync(xmlListingId, actionType));
            await task;

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Theory]
        [InlineData(ListActionType.ListNow)]
        [InlineData(ListActionType.ListCompare)]
        public async Task ProcessListingQuickCreateFails(ListActionType actionType)
        {
            var xmlListingId = Guid.NewGuid();

            var xmlListingDetailResponse = new XmlResponse.XmlListingDetailResponse()
            {
                Market = MarketCode.Austin,
                CompanyId = Factory.CompanyId,
                Name = "fakeName",
                SalesPrice = 4345355,
                State = "TX",
                StreetName = "streetName",
                StreetNum = "1234",
                Zip = "12345",
                City = "Van Alstyne",
                CommunityId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
            };

            var xmlClient = this.customWebApplicationFactory.Services.GetRequiredService<IXmlClient>();
            var xmlResourceMock = Mock.Get(xmlClient.Listing);
            xmlResourceMock.Setup(c => c.GetByIdAsync(It.Is<Guid>(x => x == Factory.XmlListingId), It.Is<bool>(x => false), It.IsAny<CancellationToken>())).ReturnsAsync(xmlListingDetailResponse);

            var companyResponse = new Response.CompanyDetail() { Id = Factory.CompanyId, Name = "fakeName" };
            var companyClient = this.customWebApplicationFactory.Services.GetRequiredService<IServiceSubscriptionClient>();
            var companyResourceMock = Mock.Get(companyClient.Company);
            companyResourceMock.Setup(c => c.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyResponse);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.Xml.ProcessListingAsync(xmlListingId, actionType));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task ListLaterAsyncSuccess()
        {
            var xmlListingId = Factory.XmlListingId;
            var listOn = DateTime.Now.AddDays(7);

            // Act
            var task = Task.Run(() => this.quicklisterAborClient.Xml.ListLaterAsync(xmlListingId, listOn));
            await task;

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task DeleteListingAsyncSuccess()
        {
            var xmlListingId = Factory.XmlListingId;
            var xmlListing = new XmlResponse.XmlListingDetailResponse
            {
                Id = Factory.XmlListingId,
                Market = MarketCode.Austin,
                CompanyId = Factory.CompanyId,
            };
            var xmlClient = this.customWebApplicationFactory.GetService<IXmlClient>();
            var xmlResourceMock = Mock.Get(xmlClient.Listing);
            xmlResourceMock
                .Setup(s => s.GetByIdAsync(It.Is<Guid>(id => id == Factory.XmlListingId), It.Is<bool>(skipProfiles => !skipProfiles), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlListing)
                .Verifiable();

            // Act
            await this.quicklisterAborClient.Xml.DeleteListingAsync(xmlListingId);

            // Assert
            xmlResourceMock.Verify();
            xmlResourceMock.Verify(
                listing => listing.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(listAction => listAction.Type == Xml.Domain.Enums.ListActionType.ListNever),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RestoreListingAsyncSuccess()
        {
            var xmlListingId = Guid.NewGuid();
            var xmlListing = new XmlResponse.XmlListingDetailResponse
            {
                Id = xmlListingId,
                Market = MarketCode.Austin,
                CompanyId = Factory.CompanyId,
            };
            var xmlClient = this.customWebApplicationFactory.GetService<IXmlClient>();
            var xmlResourceMock = Mock.Get(xmlClient.Listing);

            xmlResourceMock.Setup(s => s.GetByIdAsync(It.Is<Guid>(x => x == xmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlListing);

            // Act
            await this.quicklisterAborClient.Xml.RestoreListingAsync(xmlListingId);

            // Assert
            xmlResourceMock.Verify(r => r.RestoreListing(It.Is<Guid>(id => id == xmlListingId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteXmlListingAsyncSuccess()
        {
            var xmlListingId = Guid.NewGuid();
            var xmlListing = new XmlResponse.XmlListingDetailResponse
            {
                Id = xmlListingId,
                Market = MarketCode.Austin,
                CompanyId = Factory.CompanyId,
            };
            var xmlClient = this.customWebApplicationFactory.GetService<IXmlClient>();
            var xmlResourceMock = Mock.Get(xmlClient.Listing);

            xmlResourceMock.Setup(s => s.GetByIdAsync(It.Is<Guid>(x => x == xmlListingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlListing);

            // Act
            await this.quicklisterAborClient.Xml.DeleteListingAsync(xmlListingId);

            // Assert
            xmlResourceMock.Verify(
                r => r.ProcessListing(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<ListActionRequest>(r => r.Type == Xml.Domain.Enums.ListActionType.ListNever),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        //// Commented because for some reason fails when all tests are runned toghether but pass when it's runned alone.
        ////[Fact]
        ////public async Task CommunityGetAsyncSuccess()
        ////{
        ////    // Arrange
        ////    var filter = new CommunityRequest.CommunityRequestFilter();

        ////    // Act
        ////    var communities = await this.quicklisterAborClient.SaleCommunity.GetAsync(filter);

        ////    // Assert
        ////    Assert.True(!communities.Any(x => x.Id == Factory.CommunityAwaitingApprovalId));
        ////}

        [Fact]
        public async Task ApproveCommunitySuccess()
        {
            var communityId = Factory.CommunityAwaitingApprovalId;

            // Act
            var task = Task.Run(() => this.quicklisterAborClient.SaleCommunity.ApproveCommunity(communityId));
            await task;

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task ApproveCommunityNotFound()
        {
            var communityId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleCommunity.ApproveCommunity(communityId));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task ApproveCommunityInvalid()
        {
            var communityId = Factory.CommunityId;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => this.quicklisterAborClient.SaleCommunity.ApproveCommunity(communityId));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task CreateListingSuccessAsync()
        {
            // Arrange
            var listingDto = new Request.ListingSaleRequest
            {
                MlsStatus = MarketStatuses.Active,
                City = Cities.LaGrange,
                StreetNumber = "1234",
                StreetName = Faker.Address.StreetName(),
                ZipCode = Faker.Address.ZipCode()[..5],
                State = States.Texas,
                County = Counties.Angelina,
                CommunityId = Factory.CommunityId,
                CompanyId = Factory.CompanyId,
            };

            // Act
            var listingId = await this.quicklisterAborClient.SaleListing.CreateListing(listingDto);

            // Assert
            var createListing = await this.quicklisterAborClient.SaleListing.GetByIdAsync(listingId);
            Assert.Equal(listingDto.StreetNumber, createListing.SaleProperty.AddressInfo.StreetNumber);
            Assert.Equal(listingDto.StreetName, createListing.SaleProperty.AddressInfo.StreetName);
            Assert.Equal(listingDto.ZipCode, createListing.SaleProperty.AddressInfo.ZipCode);
            Assert.Equal(listingDto.State, createListing.SaleProperty.AddressInfo.State);
        }

        [Fact]
        public async Task GetListingsWithOpenHouseAsync()
        {
            // Arrange
            var take = 1;
            var filter = new BaseFilterRequest()
            {
                Skip = 0,
                Take = take,
            };

            // Act
            var response = await this.quicklisterAborClient.SaleListing.GetListingsWithOpenHouse(filter);

            // Assert
            Assert.NotEmpty(response.Data);
            Assert.True(response.Total == take);
        }

        private sealed class DataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> data = new()
            {
                new object[] { ListActionType.ListNow, 120000M },
                new object[] { ListActionType.ListCompare, 950000M },
                new object[] { ListActionType.ListCompare, null },
            };

            public IEnumerator<object[]> GetEnumerator() => this.data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
