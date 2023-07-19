namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SpacesDimensionsQueryResult
    {
        public CategoryType TypeCategory { get; set; }
        public Stories? Stories { get; set; }
        public int? SqFtTotal { get; set; }
        public SqFtSource? SqFtSource { get; set; }
        public ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
        public int? NumBedrooms { get; set; }
        public int? BathsFull { get; set; }
        public int? BathsHalf { get; set; }
        public IEnumerable<GarageDescription> GarageDescription { get; set; }
        public ICollection<OtherParking> OtherParking { get; set; }
    }
}
