namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class RoomDto
    {
        public Guid Id { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public RoomLevel Level { get; set; }

        public RoomType RoomType { get; set; }

        public ICollection<RoomFeatures> Features { get; set; }
    }
}
