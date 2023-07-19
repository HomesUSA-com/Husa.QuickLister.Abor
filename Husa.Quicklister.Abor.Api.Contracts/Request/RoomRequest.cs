namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class RoomRequest
    {
        public Guid Id { get; set; }

        [Range(4, 99, ErrorMessage = "{0} must be between {1} and {2}")]
        public int Length { get; set; }

        [Range(4, 99, ErrorMessage = "{0} must be between {1} and {2}")]
        public int Width { get; set; }
        public RoomLevel Level { get; set; }
        public RoomType RoomType { get; set; }
        public ICollection<RoomFeatures> Features { get; set; }
    }
}
