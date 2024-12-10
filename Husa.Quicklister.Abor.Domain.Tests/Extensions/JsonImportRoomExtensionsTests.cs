namespace Husa.Quicklister.Abor.Domain.Tests.Extensions
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Xunit;
    using JsonRoomType = Husa.JsonImport.Domain.Enums.RoomType;

    public class JsonImportRoomExtensionsTests
    {
        [Theory]
        [InlineData(JsonRoomType.GameRoom, RoomType.Game)]
        [InlineData(JsonRoomType.MediaRoom, RoomType.MediaRoom)]
        [InlineData(JsonRoomType.BonusRoom, RoomType.Bonus)]
        [InlineData(JsonRoomType.DiningRoom, RoomType.Dining)]
        [InlineData(JsonRoomType.KitchenRoom, RoomType.Kitchen)]
        [InlineData(JsonRoomType.MasterBedroom, RoomType.PrimaryBedroom)]
        [InlineData(JsonRoomType.Bedroom, RoomType.Bedroom)]
        [InlineData(JsonRoomType.OfficeRoom, RoomType.Office)]
        [InlineData(JsonRoomType.LivingRoom, RoomType.Living)]
        [InlineData(JsonRoomType.Bathroom, RoomType.Bathroom)]
        public void JsonRoomType_To_RoomType(JsonRoomType jsonStatus, RoomType marketStatus)
        {
            var value = jsonStatus.ToMarket();
            Assert.Equal(marketStatus, value);
        }
    }
}
