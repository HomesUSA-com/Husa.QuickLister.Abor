namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.JsonImport.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Application.Tests.Providers;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class ListingJsonImportServiceTests
    {
        private readonly Mock<IListingSaleRepository> listingRepository = new();
        private readonly Mock<ILogger<ListingJsonImportService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IJsonImportClient> jsonClient = new();
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();
        private readonly Mock<ISaleListingService> listingService = new();

        public ListingJsonImportServiceTests()
        {
            var jsonListingClientMock = new Mock<IJsonImportSpec>();
            this.jsonClient.SetupGet(x => x.Spec).Returns(jsonListingClientMock.Object);
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.Sut = new ListingJsonImportService(
                this.listingRepository.Object,
                this.communityRepository.Object,
                this.userContextProvider.Object,
                this.jsonClient.Object,
                this.listingService.Object,
                this.logger.Object);
        }

        public ListingJsonImportService Sut { get; set; }

        [Theory]
        [InlineData(ImportActionType.ListNow)]
        [InlineData(ImportActionType.ListCompare)]
        public async Task ImportListing_CreateNewListing_Success(ImportActionType actionType)
        {
            // Arrange
            var jsonListingId = Guid.NewGuid();

            this.jsonClient
                .Setup(x => x.Spec.GetById(It.Is<Guid>(id => id == jsonListingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetListingResponse(jsonListingId))
                .Verifiable();
            this.SetupListingQuickCreate();

            // Act
            await this.Sut.ProcessListingAsync(jsonListingId, actionType);

            // Assert
            this.listingRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
            this.listingRepository.Verify(r => r.Attach(It.IsAny<SaleListing>()), Times.Once);
            this.listingRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        private static SpecDetailResponse GetListingResponse(Guid jsonListingId, Guid? qlId = null) => new()
        {
            Id = jsonListingId,
            QuicklisterId = qlId,
            QlCompanyId = Guid.NewGuid(),
            QlPlanId = Guid.NewGuid(),
            QlCommunityId = Guid.NewGuid(),
            Location = new()
            {
                StreetName = "StreetName",
                StreetNum = "123",
            },
            Bathrooms = 2,
            Stories = 3,
            HalfBaths = 4,
            Bedrooms = 5,
            LivingAreas = 6,
            ListStatus = ListStatus.Pending,
            StatusFields = new()
            {
                ContractDate = DateTime.UtcNow,
                AcceptableFinancing = new[] { AcceptableFinancing.Cash, AcceptableFinancing.USDA },
            },
            ConstructionStage = ConstructionStage.Complete,
            OpenHouses = JsonModelProviders.GetOpenHouses(),
            Rooms = JsonModelProviders.GetRooms(),
        };

        private void SetupListingQuickCreate()
        {
            var quickCreateResult = ListingTestProvider.GetListingEntity();
            var commandResult = CommandResult<SaleListing>.Success(quickCreateResult);
            this.listingService
                .Setup(l => l.QuickCreateAsync(
                    It.IsAny<QuickCreateListingDto>(),
                    It.Is<bool>(importFromListing => !importFromListing)))
                .ReturnsAsync(commandResult);
        }
    }
}
