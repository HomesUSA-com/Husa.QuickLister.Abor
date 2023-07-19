namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideRoomInfo
    {
        int Length { get; set; }

        int Width { get; set; }

        RoomLevel Level { get; set; }

        RoomType RoomType { get; set; }

        ICollection<RoomFeatures> Features { get; set; }
    }
}
