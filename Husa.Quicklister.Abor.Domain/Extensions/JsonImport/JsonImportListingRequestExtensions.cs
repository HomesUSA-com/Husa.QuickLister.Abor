namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using System;
    using System.Collections.Generic;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public static class JsonImportListingRequestExtensions
    {
        public static void Import(this SaleListingRequest listing, SpecDetailResponse spec)
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
                listing.SaleProperty.ImportRooms(rooms);
            }

            listing.SaleProperty.UpdateOpenHouse(spec.OpenHouses.ToOpenHouses());
            if (listing.SaleProperty.OpenHouses.Count > 0)
            {
                listing.SaleProperty.ShowingInfo.EnableOpenHouse();
            }
        }

        private static void ImportRooms<T>(this SalePropertyRecord salePropertyRecord, IEnumerable<T> rooms)
            where T : Room
        {
            salePropertyRecord.Rooms.Clear();

            foreach (var roomDetail in rooms)
            {
                var room = new RoomRecord()
                {
                    SalePropertyId = salePropertyRecord.Id,
                    RoomType = roomDetail.RoomType,
                    Level = roomDetail.Level,
                    Description = roomDetail.Description,
                    Features = roomDetail.Features,
                };

                salePropertyRecord.Rooms.Add(room);
            }
        }
    }
}
