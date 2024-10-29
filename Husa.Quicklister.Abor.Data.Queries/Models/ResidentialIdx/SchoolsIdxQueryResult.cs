namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsIdxQueryResult
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }
    }
}
