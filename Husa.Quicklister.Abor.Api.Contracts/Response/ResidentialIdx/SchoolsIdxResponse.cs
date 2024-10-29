namespace Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsIdxResponse
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }
    }
}
