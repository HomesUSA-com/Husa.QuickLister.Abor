namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using MigrationBillingType = Husa.Migration.Crosscutting.Enums.BillingType;
    using MigrationOpenHouseType = Husa.Migration.Crosscutting.Enums.OpenHouseType;
    using MigrationRoomType = Husa.Migration.Crosscutting.Enums.RoomType;

    public static class MigrationExtensions
    {
        public static OpenHouseType ToOpenHouseType(this MigrationOpenHouseType type) => type switch
        {
            MigrationOpenHouseType.Monday => OpenHouseType.Monday,
            MigrationOpenHouseType.Tuesday => OpenHouseType.Tuesday,
            MigrationOpenHouseType.Wednesday => OpenHouseType.Wednesday,
            MigrationOpenHouseType.Thursday => OpenHouseType.Thursday,
            MigrationOpenHouseType.Friday => OpenHouseType.Friday,
            MigrationOpenHouseType.Saturday => OpenHouseType.Saturday,
            MigrationOpenHouseType.Sunday => OpenHouseType.Sunday,
            _ => OpenHouseType.Monday,
        };

        public static RoomType ToRoomType(this MigrationRoomType type) => type switch
        {
            MigrationRoomType.Dining => RoomType.Dining,
            MigrationRoomType.MasterBedroom => RoomType.PrimaryBedroom,
            MigrationRoomType.Bed => RoomType.Bedroom,
            MigrationRoomType.MasterBath => RoomType.PrimaryBathroom,
            MigrationRoomType.FullBath or MigrationRoomType.HalfBath => RoomType.Bathroom,
            MigrationRoomType.Kitchen => RoomType.Kitchen,
            MigrationRoomType.Bonus => RoomType.Bonus,
            MigrationRoomType.Family => RoomType.FamilyRoom,
            MigrationRoomType.Game => RoomType.Game,
            MigrationRoomType.Laundry => RoomType.Laundry,
            MigrationRoomType.Library => RoomType.Library,
            MigrationRoomType.Loft => RoomType.Loft,
            MigrationRoomType.Living => RoomType.Living,
            MigrationRoomType.Media => RoomType.MediaRoom,
            MigrationRoomType.Office => RoomType.Office,
            _ => throw new NotImplementedException(),
        };

        public static ActionType ToActionType(this MigrationBillingType type) =>
            type switch
            {
                MigrationBillingType.NewListing => ActionType.NewListing,
                MigrationBillingType.Comparable => ActionType.Comparable,
                MigrationBillingType.PendingTransfer => ActionType.PendingTransfer,
                MigrationBillingType.Relist => ActionType.Relist,
                MigrationBillingType.Transfer => ActionType.ActiveTransfer,
                _ => throw new NotImplementedException(),
            };
    }
}
