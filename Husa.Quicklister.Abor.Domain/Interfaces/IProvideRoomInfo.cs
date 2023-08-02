namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideRoomInfo
    {
        RoomLevel Level { get; set; }

        RoomType RoomType { get; set; }
    }
}
