namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsRequest
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public string OtherElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public string OtherMiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }

        public string OtherHighSchool { get; set; }
    }
}
