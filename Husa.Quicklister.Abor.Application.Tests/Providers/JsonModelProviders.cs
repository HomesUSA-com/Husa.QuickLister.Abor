namespace Husa.Quicklister.Abor.Application.Tests.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Husa.JsonImport.Api.Contracts.Response;
    using JsonRoomType = JsonImport.Domain.Enums.RoomType;

    public static class JsonModelProviders
    {
        public static ICollection<RoomResponse> GetRooms() =>
            new RoomResponse[]
            {
                new()
                {
                    Type = JsonRoomType.KitchenRoom,
                    Dimensions = "5x6",
                    Level = 1,
                },
                new()
                {
                    Type = JsonRoomType.Bathroom,
                    Dimensions = "4x4",
                    Level = 1,
                },
                new()
                {
                    Type = JsonRoomType.Bedroom,
                    Dimensions = "8x8",
                    Level = 1,
                },
            };

        public static ICollection<OpenHouseResponse> GetOpenHouses() =>
            new OpenHouseResponse[]
            {
                new()
                {
                    Day = DayOfWeek.Friday,
                    StartTime = TimeSpan.Parse("08:15", CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.Parse("10:00", CultureInfo.InvariantCulture),
                },
                new()
                {
                    Day = DayOfWeek.Saturday,
                    StartTime = TimeSpan.Parse("10:30", CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.Parse("14:00", CultureInfo.InvariantCulture),
                },
                new()
                {
                    Day = DayOfWeek.Sunday,
                    StartTime = TimeSpan.Parse("10:30", CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.Parse("14:00", CultureInfo.InvariantCulture),
                },
            };
    }
}
