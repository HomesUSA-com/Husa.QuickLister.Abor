namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx
{
    public class ResidentialIdxQueryResult : Husa.Quicklister.Extensions.Data.Queries.Models.ResidentialIdxQueryResult
    {
        public FinancialIdxQueryResult Financial { get; set; }
        public SchoolsIdxQueryResult Schools { get; set; }
        public PropertyIdxQueryResult Property { get; set; }
        public SpacesDimensionsIdxQueryResult SpacesDimensions { get; set; }
    }
}
