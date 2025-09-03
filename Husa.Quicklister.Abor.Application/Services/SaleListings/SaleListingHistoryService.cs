namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class SaleListingHistoryService : Husa.Quicklister.Extensions.Application.Services.SaleListings.SaleListingHistoryService<SaleListing, SaleListingRequest>
    {
        public SaleListingHistoryService(
            IListingSaleRepository listingRepository,
            IListingHistoryRepository historyRepository,
            ISaleListingRequestRepository requestRepository,
            ILogger<SaleListingHistoryService> logger)
            : base(listingRepository, historyRepository, requestRepository, logger)
        {
        }

        protected override void CopyProperty(SaleListing listing, SummaryField field)
        {
            if (field.FieldName.EndsWith($".{nameof(SaleListing.SaleProperty.Rooms)}"))
            {
                UndoRoomsChanges(listing, field);
                return;
            }
            else if (field.FieldName.EndsWith($".{nameof(SaleListing.SaleProperty.OpenHouses)}"))
            {
                UndoOpenHousesChanges(listing, field);
                return;
            }

            CopyPropertiesExtensions.CopyProperty(listing, field.FieldName, field.OldValue);
        }

        private static void UndoRoomsChanges(SaleListing listing, SummaryField field)
        => UndoCollectionChanges(
                field,
                listing.SaleProperty.Rooms,
                new ListingRoomComparer(),
                listing.SaleProperty.UpdateRooms);

        private static void UndoOpenHousesChanges(SaleListing listing, SummaryField field)
        => UndoCollectionChanges(
                field,
                listing.SaleProperty.OpenHouses,
                new OpenHouseComparer(),
                listing.SaleProperty.ImportOpenHouse<OpenHouse, SaleListingOpenHouse, SaleProperty>);
    }
}
