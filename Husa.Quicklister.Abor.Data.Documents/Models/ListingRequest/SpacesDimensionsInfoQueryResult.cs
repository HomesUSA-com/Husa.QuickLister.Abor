namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SpacesDimensionsInfoQueryResult
    {
        public CategoryType TypeCategory { get; set; }

        public int? SqFtTotal { get; set; }

        public SqFtSource? SqFtSource { get; set; }

        public ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }

        public ICollection<OtherParking> OtherParking { get; set; }

        public Stories Stories { get; set; }

        public int? BathsFull { get; set; }

        public int? BathsHalf { get; set; }

        public int? NumBedrooms { get; set; }

        public ICollection<GarageDescription> GarageDescription { get; set; }
    }
}
