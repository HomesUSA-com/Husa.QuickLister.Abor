namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsRequest
    {
        public SchoolDistrict? SchoolDistrict { get; set; }
        public MiddleSchool? MiddleSchool { get; set; }
        public ElementarySchool? ElementarySchool { get; set; }
        public HighSchool? HighSchool { get; set; }
    }
}
