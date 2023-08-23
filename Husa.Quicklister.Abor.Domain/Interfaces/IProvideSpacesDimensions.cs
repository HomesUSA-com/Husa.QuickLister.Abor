namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideSpacesDimensions
    {
        Stories? StoriesTotal { get; set; }
        int? SqFtTotal { get; set; }
        int? DiningAreasTotal { get; set; }
        int? MainLevelBedroomTotal { get; set; }
        int? OtherLevelsBedroomTotal { get; set; }
        int? HalfBathsTotal { get; set; }
        int? FullBathsTotal { get; set; }
        int? LivingAreasTotal { get; set; }
    }
}
