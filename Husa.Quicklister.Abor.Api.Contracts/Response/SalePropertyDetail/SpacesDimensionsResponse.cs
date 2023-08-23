namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SpacesDimensionsResponse
    {
        public CategoryType TypeCategory { get; set; }
        public SqFtSource? SqFtSource { get; set; }
        public ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
        public ICollection<OtherParking> OtherParking { get; set; }

        public Stories? StoriesTotal { get; set; }
        public int? SqFtTotal { get; set; }
        public int? DiningAreasTotal { get; set; }
        public int? MainLevelBedroomTotal { get; set; }
        public int? OtherLevelsBedroomTotal { get; set; }
        public int? HalfBathsTotal { get; set; }
        public int? FullBathsTotal { get; set; }
        public int? LivingAreasTotal { get; set; }
    }
}
