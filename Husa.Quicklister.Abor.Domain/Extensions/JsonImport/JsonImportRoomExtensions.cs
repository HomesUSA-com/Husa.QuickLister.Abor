namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using JsonRoomType = Husa.JsonImport.Domain.Enums.RoomType;

    public static class JsonImportRoomExtensions
    {
        public static ICollection<Room> ToRooms(this ICollection<RoomResponse> jsonRooms)
            => jsonRooms.Select(x => x.ToRoom()).Where(x => x != null).ToList();

        public static RoomType? ToMarket(this JsonRoomType value) => value switch
        {
            JsonRoomType.GameRoom => RoomType.Game,
            JsonRoomType.MediaRoom => RoomType.MediaRoom,
            JsonRoomType.BonusRoom => RoomType.Bonus,
            JsonRoomType.DiningRoom => RoomType.Dining,
            JsonRoomType.KitchenRoom => RoomType.Kitchen,
            JsonRoomType.MasterBedroom => RoomType.PrimaryBedroom,
            JsonRoomType.Bedroom => RoomType.Bedroom,
            JsonRoomType.OfficeRoom => RoomType.Office,
            JsonRoomType.LivingRoom => RoomType.Living,
            JsonRoomType.Bathroom => RoomType.Bathroom,
            _ => null,
        };

        private static Room ToRoom(this RoomResponse jsonRoom)
        {
            var roomType = jsonRoom.Type.ToMarket();
            if (!roomType.HasValue)
            {
                return null;
            }

            return new Room(
                roomType.Value,
                description: jsonRoom.Dimensions,
                level: jsonRoom.Level.ToMarket(),
                features: Array.Empty<RoomFeatures>());
        }

        private static RoomLevel ToMarket(this int value) => value switch
        {
            0 => RoomLevel.Lower,
            1 => RoomLevel.First,
            2 => RoomLevel.Second,
            3 => RoomLevel.Third,
            4 => RoomLevel.Upper,
            _ => RoomLevel.Main,
        };
    }
}
