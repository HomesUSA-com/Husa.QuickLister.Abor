namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideRoomInfo
    {
        RoomLevel Level { get; set; }

        RoomType RoomType { get; set; }
        ICollection<RoomFeatures> Features { get; set; }
    }
}
