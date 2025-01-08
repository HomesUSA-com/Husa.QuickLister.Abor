namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using System;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;

    public static class JsonImportListingExtensions
    {
        public static void Import(this SaleListing listing, SpecDetailResponse spec)
        {
            ArgumentNullException.ThrowIfNull(spec);
            listing.ImportRootInfo(spec);

            listing.SaleProperty.SpacesDimensionsInfo.Import(spec);
            listing.SaleProperty.PropertyInfo.Import(spec);
            listing.SaleProperty.FeaturesInfo.Import(spec);

            if (listing.MlsStatus != MarketStatuses.Active)
            {
                listing.StatusFieldsInfo.Import(spec.StatusFields);
            }

            var rooms = spec.Rooms.ToRooms();
            if (rooms.Count > 0)
            {
                listing.SaleProperty.ImportRoomsFromEntity(rooms);
            }

            listing.SaleProperty.ImportOpenHouse<OpenHouse, SaleListingOpenHouse, SaleProperty>(spec.OpenHouses.ToOpenHouses());
            if (listing.SaleProperty.OpenHouses.Count > 0)
            {
                listing.SaleProperty.ShowingInfo.EnableOpenHouse();
            }
        }
    }
}
