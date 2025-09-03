namespace Husa.Quicklister.Abor.Tests.Services.SaleListings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class SaleListingHistoryServiceTests
    {
        private readonly Mock<IListingSaleRepository> mockListingRepository;
        private readonly Mock<IListingHistoryRepository> mockHistoryRepository;
        private readonly Mock<ISaleListingRequestRepository> mockRequestRepository;
        private readonly Mock<ILogger<SaleListingHistoryService>> mockLogger;
        private readonly TestSaleListingHistoryService sut;

        public SaleListingHistoryServiceTests()
        {
            this.mockListingRepository = new Mock<IListingSaleRepository>();
            this.mockHistoryRepository = new Mock<IListingHistoryRepository>();
            this.mockRequestRepository = new Mock<ISaleListingRequestRepository>();
            this.mockLogger = new Mock<ILogger<SaleListingHistoryService>>();

            this.sut = new TestSaleListingHistoryService(
                this.mockListingRepository.Object,
                this.mockHistoryRepository.Object,
                this.mockRequestRepository.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public void CopyProperty_WithRoomChanges_ShouldCopyRoomsCorrectly()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.SaleProperty.Rooms = [RoomTestProvider.GetListingSaleRoomEntity(), RoomTestProvider.GetListingSaleRoomEntity()];
            var oldRooms = new List<Room>
            {
                RoomTestProvider.GetListingSaleRoomEntity(),
            };
            var newRooms = listing.SaleProperty.Rooms.ToList();

            var field = new SummaryField
            {
                FieldName = $"SaleProperty.{nameof(SaleListing.SaleProperty.Rooms)}",
                OldValue = JsonSerializer.Serialize(oldRooms),
                NewValue = JsonSerializer.Serialize(newRooms),
            };

            // Act
            this.sut.TestCopyProperty(listing, field);

            // Assert
            Assert.Equal(oldRooms.Count, listing.SaleProperty.Rooms.Count);
        }

        [Fact]
        public void CopyProperty_WithOpenHouseChanges_ShouldCopyOpenHousesCorrectly()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.SaleProperty.OpenHouses = [TestModelProvider.GetListingSaleOpenHouse(), TestModelProvider.GetListingSaleOpenHouse()];
            var oldOpenHouses = new List<OpenHouse>
            {
                TestModelProvider.GetListingSaleOpenHouse(),
            };
            var newOpenHouses = listing.SaleProperty.OpenHouses.ToList();

            var field = new SummaryField
            {
                FieldName = $"SaleProperty.{nameof(SaleListing.SaleProperty.OpenHouses)}",
                OldValue = JsonSerializer.Serialize(oldOpenHouses),
                NewValue = JsonSerializer.Serialize(newOpenHouses),
            };

            // Act
            this.sut.TestCopyProperty(listing, field);

            // Assert
            Assert.Equal(oldOpenHouses.Count, listing.SaleProperty.OpenHouses.Count);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class TestSaleListingHistoryService : SaleListingHistoryService
#pragma warning restore SA1402 // File may only contain a single type
    {
        public TestSaleListingHistoryService(
            IListingSaleRepository listingRepository,
            IListingHistoryRepository historyRepository,
            ISaleListingRequestRepository requestRepository,
            ILogger<SaleListingHistoryService> logger)
            : base(listingRepository, historyRepository, requestRepository, logger)
        {
        }

        public void TestCopyProperty(SaleListing listing, SummaryField field)
        {
            this.CopyProperty(listing, field);
        }
    }
}
