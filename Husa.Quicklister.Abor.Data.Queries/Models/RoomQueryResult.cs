namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class RoomQueryResult
    {
        public Guid Id { get; set; }
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<RoomFeatures> Features { get; set; }
    }
}
