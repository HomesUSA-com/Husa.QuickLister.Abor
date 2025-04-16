namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using Husa.Xml.Api.Contracts.Response;
    using Husa.Xml.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class XmlMediaServiceTests
    {
        private readonly Mock<IXmlClientWithoutToken> xmlClient = new();
        private readonly Mock<IPlanRepository> planRepository = new();
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IProvideTraceId> traceIdProvider = new();
        private readonly Mock<IPlanMediaService> planMediaService = new();
        private readonly Mock<ICommunityMediaService> communityMediaService = new();
        private readonly Mock<ISaleListingMediaService> saleListingMediaService = new();
        private readonly Mock<ILogger<CommunityXmlMediaService>> communityLogger = new();
        private readonly Mock<ILogger<PlanXmlMediaService>> planLogger = new();
        private readonly Mock<ILogger<SaleListingXmlMediaService>> listingLogger = new();

        public XmlMediaServiceTests(ApplicationServicesFixture fixture)
        {
        }

        [Fact]
        public async Task ImportXmlSubdivisionMediaXmlSubdivisionNotFound()
        {
            // Arrange
            var xmlSubdivisionId = Guid.NewGuid();
            var subdivisionResource = new Mock<IXmlSubdivision>();
            subdivisionResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlSubdivisionId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException(
                    message: $"The id '{xmlSubdivisionId}' wasn't found for the type",
                    inner: null,
                    statusCode: HttpStatusCode.NotFound));
            this.xmlClient.SetupGet(c => c.Subdivision).Returns(subdivisionResource.Object).Verifiable();

            var sut = new CommunityXmlMediaService(
               this.xmlClient.Object,
               this.communityMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.communityLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => sut.ImportSubdivisionMedia(xmlSubdivisionId, maxImagesAllowed: 50));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            this.xmlClient.Verify();
        }

        [Fact]
        public async Task ImportXmlSubdivisionMediaXmlSubdivisionNotImported()
        {
            // Arrange
            var xmlSubdivisionId = Guid.NewGuid();
            var subdivisionResource = new Mock<IXmlSubdivision>();
            subdivisionResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlSubdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubdivisionResponse { Id = xmlSubdivisionId })
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Subdivision).Returns(subdivisionResource.Object).Verifiable();

            var sut = new CommunityXmlMediaService(
               this.xmlClient.Object,
               this.communityMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.communityLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => sut.ImportSubdivisionMedia(xmlSubdivisionId, maxImagesAllowed: 50));

            // Assert
            Assert.StartsWith("Cannot process the media import of the xml subdivision", exception.Message);
            this.xmlClient.Verify();
            subdivisionResource.Verify();
        }

        [Fact]
        public async Task ImportXmlSubdivisionMediaCommunityProfileDoesntExist()
        {
            // Arrange
            var xmlSubdivisionId = Guid.NewGuid();
            var communityProfileId = Guid.NewGuid();
            var subdivisionResource = new Mock<IXmlSubdivision>();
            subdivisionResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlSubdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubdivisionResponse
                {
                    Id = xmlSubdivisionId,
                    CommunityProfileId = communityProfileId,
                })
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Subdivision).Returns(subdivisionResource.Object).Verifiable();

            var sut = new CommunityXmlMediaService(
                this.xmlClient.Object,
                this.communityMediaService.Object,
                this.planRepository.Object,
                this.communitySaleRepository.Object,
                this.listingSaleRepository.Object,
                this.userContextProvider.Object,
                this.traceIdProvider.Object,
                this.communityLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.ImportSubdivisionMedia(xmlSubdivisionId, maxImagesAllowed: 50));

            // Assert
            Assert.Equal(communityProfileId, exception.Id);
            this.xmlClient.Verify();
            subdivisionResource.Verify();
        }

        [Fact]
        public async Task ImportXmlSubdivisionMediaXmlSubdivisionHasNoMedia()
        {
            // Arrange
            var xmlSubdivisionId = Guid.NewGuid();
            var communityProfileId = Guid.NewGuid();
            var subdivisionResource = new Mock<IXmlSubdivision>();
            subdivisionResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlSubdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubdivisionResponse
                {
                    Id = xmlSubdivisionId,
                    CommunityProfileId = communityProfileId,
                })
                .Verifiable();
            subdivisionResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlSubdivisionId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ImageResponse>())
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Subdivision).Returns(subdivisionResource.Object).Verifiable();

            var community = new Mock<CommunitySale>();
            community.SetupGet(c => c.Id).Returns(communityProfileId);
            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityProfileId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync(community.Object);

            var sut = new CommunityXmlMediaService(
               this.xmlClient.Object,
               this.communityMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.communityLogger.Object);

            // Act
            await sut.ImportSubdivisionMedia(xmlSubdivisionId, maxImagesAllowed: 50);

            // Assert
            this.userContextProvider.Verify(c => c.GetCurrentUserId(), Times.Never);
            this.traceIdProvider.VerifyGet(tp => tp.TraceId, Times.Never);
            this.xmlClient.Verify();
            subdivisionResource.Verify();
            subdivisionResource
                .Verify(
                    c => c.SubdivisionImagesImported(
                        It.Is<Guid>(id => id == xmlSubdivisionId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task ImportXmlSubdivisionMediaSuccess()
        {
            // Arrange
            var xmlSubdivisionId = Guid.NewGuid();
            var communityProfileId = Guid.NewGuid();
            var subdivisionResource = new Mock<IXmlSubdivision>();
            var userId = Guid.NewGuid();
            var traceId = Guid.NewGuid().ToString();
            subdivisionResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlSubdivisionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubdivisionResponse
                {
                    Id = xmlSubdivisionId,
                    CommunityProfileId = communityProfileId,
                }).Verifiable();

            subdivisionResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlSubdivisionId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetResultImages()).Verifiable();

            this.xmlClient.SetupGet(c => c.Subdivision).Returns(subdivisionResource.Object).Verifiable();
            var community = new Mock<CommunitySale>();
            community.SetupGet(c => c.Id).Returns(communityProfileId);
            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityProfileId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync(community.Object);
            this.userContextProvider.Setup(c => c.GetCurrentUserId()).Returns(userId).Verifiable();
            this.traceIdProvider.SetupGet(tp => tp.TraceId).Returns(traceId).Verifiable();

            var sut = new CommunityXmlMediaService(
               this.xmlClient.Object,
               this.communityMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.communityLogger.Object);

            // Act
            await sut.ImportSubdivisionMedia(xmlSubdivisionId, maxImagesAllowed: 50);

            // Assert
            this.traceIdProvider.Verify();
            this.userContextProvider.Verify();
            this.xmlClient.Verify();
            subdivisionResource.Verify();
            subdivisionResource
                .Verify(
                    c => c.SubdivisionImagesImported(
                        It.Is<Guid>(id => id == xmlSubdivisionId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task ImportXmlPlanMediaXmlPlanNotFound()
        {
            // Arrange
            var xmlPlanId = Guid.NewGuid();
            var planResource = new Mock<IXmlPlan>();
            planResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException(
                    message: $"The id '{xmlPlanId}' wasn't found for the type",
                    inner: null,
                    statusCode: HttpStatusCode.NotFound));
            this.xmlClient.SetupGet(c => c.Plan).Returns(planResource.Object).Verifiable();

            var sut = new CommunityXmlMediaService(
               this.xmlClient.Object,
               this.communityMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.communityLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => sut.ImportPlanMedia(xmlPlanId, maxImagesAllowed: 50));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            this.xmlClient.Verify();
        }

        [Fact]
        public async Task ImportXmlPlanMediaXmlPlanNotImported()
        {
            // Arrange
            var xmlPlanId = Guid.NewGuid();
            var planResource = new Mock<IXmlPlan>();
            planResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanResponse { Id = xmlPlanId })
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Plan).Returns(planResource.Object).Verifiable();

            var sut = new PlanXmlMediaService(
               this.xmlClient.Object,
               this.planMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.planLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => sut.ImportPlanMedia(xmlPlanId, maxImagesAllowed: 50));

            // Assert
            Assert.StartsWith("Cannot process the media import of the xml plan", exception.Message);
            this.xmlClient.Verify();
            planResource.Verify();
        }

        [Fact]
        public async Task ImportXmlPlanMediaPlanProfileDoesntExist()
        {
            // Arrange
            var xmlPlanId = Guid.NewGuid();
            var planProfileId = Guid.NewGuid();
            var planResource = new Mock<IXmlPlan>();
            planResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanResponse
                {
                    Id = xmlPlanId,
                    PlanProfileId = planProfileId,
                })
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Plan).Returns(planResource.Object).Verifiable();

            var sut = new PlanXmlMediaService(
                this.xmlClient.Object,
                this.planMediaService.Object,
                this.planRepository.Object,
                this.communitySaleRepository.Object,
                this.listingSaleRepository.Object,
                this.userContextProvider.Object,
                this.traceIdProvider.Object,
                this.planLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException<Plan>>(() => sut.ImportPlanMedia(xmlPlanId, maxImagesAllowed: 50));

            // Assert
            Assert.Equal(planProfileId, exception.Id);
            this.xmlClient.Verify();
            planResource.Verify();
        }

        [Fact]
        public async Task ImportXmlPlanMediaXmlPlanHasNoMedia()
        {
            // Arrange
            var xmlPlanId = Guid.NewGuid();
            var planProfileId = Guid.NewGuid();
            var planResource = new Mock<IXmlPlan>();
            planResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanResponse
                {
                    Id = xmlPlanId,
                    PlanProfileId = planProfileId,
                })
                .Verifiable();
            planResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlPlanId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ImageResponse>())
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Plan).Returns(planResource.Object).Verifiable();

            var plan = new Mock<Plan>();
            plan.SetupGet(c => c.Id).Returns(planProfileId);
            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planProfileId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync(plan.Object);

            var sut = new PlanXmlMediaService(
               this.xmlClient.Object,
               this.planMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.planLogger.Object);

            // Act
            await sut.ImportPlanMedia(xmlPlanId, maxImagesAllowed: 50);

            // Assert
            this.userContextProvider.Verify(c => c.GetCurrentUserId(), Times.Never);
            this.traceIdProvider.VerifyGet(tp => tp.TraceId, Times.Never);
            this.xmlClient.Verify();
            planResource.Verify();
            planResource
                .Verify(
                    c => c.PlanImagesImported(
                        It.Is<Guid>(id => id == xmlPlanId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task ImportXmlPlanMediaSuccess()
        {
            // Arrange
            var xmlPlanId = Guid.NewGuid();
            var planProfileId = Guid.NewGuid();
            var planResource = new Mock<IXmlPlan>();
            var userId = Guid.NewGuid();
            var traceId = Guid.NewGuid().ToString();
            planResource
                .Setup(c => c.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanResponse
                {
                    Id = xmlPlanId,
                    PlanProfileId = planProfileId,
                }).Verifiable();

            planResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlPlanId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetResultImages()).Verifiable();

            this.xmlClient.SetupGet(c => c.Plan).Returns(planResource.Object).Verifiable();
            var plan = new Mock<Plan>();
            plan.SetupGet(c => c.Id).Returns(planProfileId);
            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planProfileId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync(plan.Object);
            this.userContextProvider.Setup(c => c.GetCurrentUserId()).Returns(userId).Verifiable();
            this.traceIdProvider.SetupGet(tp => tp.TraceId).Returns(traceId).Verifiable();

            var sut = new PlanXmlMediaService(
               this.xmlClient.Object,
               this.planMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.planLogger.Object);

            // Act
            await sut.ImportPlanMedia(xmlPlanId, maxImagesAllowed: 50);

            // Assert
            this.traceIdProvider.Verify();
            this.userContextProvider.Verify();
            this.xmlClient.Verify();
            planResource.Verify();
            planResource
                .Verify(
                    c => c.PlanImagesImported(
                        It.Is<Guid>(id => id == xmlPlanId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task ImportXmlListingMediaSaleListingDoesntExist()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();

            var sut = new SaleListingXmlMediaService(
               this.xmlClient.Object,
               this.saleListingMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.listingLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => sut.ImportListingMedia(xmlListingId));
        }

        [Fact]
        public async Task ImportXmlListingMediaXmlListingHasNoMedia()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();

            listingResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ImageResponse>())
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Listing).Returns(listingResource.Object).Verifiable();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(c => c.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(c => c.GetListingByXmlListingId(It.Is<Guid>(id => id == xmlListingId)))
                .ReturnsAsync(listing.Object);

            var sut = new SaleListingXmlMediaService(
                this.xmlClient.Object,
                this.saleListingMediaService.Object,
                this.planRepository.Object,
                this.communitySaleRepository.Object,
                this.listingSaleRepository.Object,
                this.userContextProvider.Object,
                this.traceIdProvider.Object,
                this.listingLogger.Object);

            // Act
            await sut.ImportListingMedia(xmlListingId);

            // Assert
            this.userContextProvider.Verify(c => c.GetCurrentUserId(), Times.Never);
            this.traceIdProvider.VerifyGet(tp => tp.TraceId, Times.Never);
            this.xmlClient.Verify();
            listingResource.Verify();
            listingResource
                .Verify(
                    c => c.ListingImagesImported(
                        It.Is<Guid>(id => id == xmlListingId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task ImportXmlListingMediaXmlListingCheckingMediaLimit()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();
            var imageResponse = new List<ImageResponse>()
            {
                new ImageResponse() { Id = Guid.NewGuid(), },
            };

            listingResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(imageResponse)
                .Verifiable();
            this.xmlClient.SetupGet(c => c.Listing).Returns(listingResource.Object).Verifiable();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(c => c.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(c => c.GetListingByXmlListingId(It.Is<Guid>(id => id == xmlListingId)))
                .ReturnsAsync(listing.Object);

            var sut = new SaleListingXmlMediaService(
               this.xmlClient.Object,
               this.saleListingMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.listingLogger.Object);

            // Act
            await sut.ImportListingMedia(xmlListingId, checkMediaLimit: true, maxImagesAllowed: 45);

            // Assert
            this.userContextProvider.Verify(c => c.GetCurrentUserId(), Times.Once);
            this.traceIdProvider.VerifyGet(tp => tp.TraceId, Times.Once);
            this.xmlClient.Verify();
            listingResource.Verify();
            listingResource
                .Verify(
                    c => c.ListingImagesImported(
                        It.Is<Guid>(id => id == xmlListingId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task ImportXmlListingMediaSuccess()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();
            var userId = Guid.NewGuid();
            var traceId = Guid.NewGuid().ToString();

            listingResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetResultImages()).Verifiable();

            this.xmlClient.SetupGet(c => c.Listing).Returns(listingResource.Object).Verifiable();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.XmlListingId = xmlListingId;
            this.listingSaleRepository
                .Setup(c => c.GetListingByXmlListingId(It.Is<Guid>(id => id == xmlListingId)))
                .ReturnsAsync(listing);
            this.userContextProvider.Setup(c => c.GetCurrentUserId()).Returns(userId).Verifiable();
            this.traceIdProvider.SetupGet(tp => tp.TraceId).Returns(traceId).Verifiable();

            var sut = new SaleListingXmlMediaService(
               this.xmlClient.Object,
               this.saleListingMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.listingLogger.Object);

            // Act
            await sut.ImportListingMedia(xmlListingId);

            // Assert
            this.traceIdProvider.Verify();
            this.userContextProvider.Verify();
            this.xmlClient.Verify();
            listingResource.Verify();
            listingResource
                .Verify(
                    c => c.ListingImagesImported(
                        It.Is<Guid>(id => id == xmlListingId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task ImportXmlListingMediaSuccessSync()
        {
            // Arrange
            var xmlListingId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingResource = new Mock<IXmlListing>();

            listingResource
                .Setup(c => c.Media(
                    It.Is<Guid>(id => id == xmlListingId),
                    It.Is<bool>(excludeImported => excludeImported),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetResultImages()).Verifiable();

            this.xmlClient.SetupGet(c => c.Listing).Returns(listingResource.Object).Verifiable();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.XmlListingId = xmlListingId;
            this.listingSaleRepository
                .Setup(c => c.GetListingByXmlListingId(It.Is<Guid>(id => id == xmlListingId)))
                .ReturnsAsync(listing);
            this.saleListingMediaService
                .Setup(c => c.CreateAsync(
                    It.IsAny<IEnumerable<SimpleMedia>>()))
                .Verifiable();

            var sut = new SaleListingXmlMediaService(
               this.xmlClient.Object,
               this.saleListingMediaService.Object,
               this.planRepository.Object,
               this.communitySaleRepository.Object,
               this.listingSaleRepository.Object,
               this.userContextProvider.Object,
               this.traceIdProvider.Object,
               this.listingLogger.Object);

            // Act
            await sut.ImportListingMedia(xmlListingId, checkMediaLimit: false, useServiceBus: false);

            // Assert
            this.saleListingMediaService.Verify();
            this.xmlClient.Verify();
            listingResource.Verify();
            listingResource
                .Verify(
                    c => c.ListingImagesImported(
                        It.Is<Guid>(id => id == xmlListingId),
                        It.IsAny<ImportedImagesRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        private static IEnumerable<ImageResponse> GetResultImages(int totalImages = 10)
        {
            for (int index = 1; index <= totalImages; index++)
            {
                yield return new ImageResponse
                {
                    Id = Guid.NewGuid(),
                    Caption = $"Image Caption {index}",
                    Discriminator = Discriminator.Plan,
                    Reference = $"https://localhost/some-url-{index}",
                    Title = $"Image Title {index}",
                    SequencePosition = index,
                };
            }
        }
    }
}
