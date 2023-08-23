namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class RoomTestProvider
    {
        public static ListingSaleRoom GetListingSaleRoomEntity(Guid? salePropertyId = null)
        {
            var type = Faker.Enum.Random<RoomType>();
            var level = Faker.Enum.Random<RoomLevel>();

            return new ListingSaleRoom(salePropertyId ?? Guid.NewGuid(), type, level);
        }

        public static ICollection<ListingSaleRoom> GetListingSaleRooms(Guid? salePropertyId = null, int? totalElements = 4)
        {
            var data = new List<ListingSaleRoom>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetListingSaleRoomEntity(salePropertyId));
            }

            return data;
        }
    }
}
