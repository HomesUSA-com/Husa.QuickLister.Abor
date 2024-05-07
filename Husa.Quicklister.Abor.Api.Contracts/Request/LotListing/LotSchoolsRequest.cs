namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotSchoolsRequest
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }
        public string OtherElementarySchool { get; set; }
        public string OtherMiddleSchool { get; set; }
        public string OtherHighSchool { get; set; }
    }
}
