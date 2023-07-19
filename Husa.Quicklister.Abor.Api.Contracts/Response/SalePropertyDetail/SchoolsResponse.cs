namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsResponse
    {
        public SchoolDistrict? SchoolDistrict { get; set; }
        public MiddleSchool? MiddleSchool { get; set; }
        public ElementarySchool? ElementarySchool { get; set; }
        public HighSchool? HighSchool { get; set; }
    }
}
