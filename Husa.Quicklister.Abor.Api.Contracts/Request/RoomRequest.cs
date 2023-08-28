namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class RoomRequest
    {
        public Guid Id { get; set; }
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public ICollection<RoomFeatures> Features { get; set; }
    }
}
