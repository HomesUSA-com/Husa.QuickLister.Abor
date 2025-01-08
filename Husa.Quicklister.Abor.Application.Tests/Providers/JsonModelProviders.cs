namespace Husa.Quicklister.Abor.Application.Tests.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.JsonImport.Domain.Enums;
    using JsonRoomType = Husa.JsonImport.Domain.Enums.RoomType;

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

        public static SpecDetailResponse GetListingResponse(Guid jsonListingId, Guid? qlId = null) => new()
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
                Financing = new[] { AcceptableFinancing.Cash, AcceptableFinancing.USDA },
            },
            ConstructionStage = ConstructionStage.Complete,
            OpenHouses = JsonModelProviders.GetOpenHouses(),
            Rooms = JsonModelProviders.GetRooms(),
        };
    }
}
