namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SpacesDimensionsRequest
    {
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
