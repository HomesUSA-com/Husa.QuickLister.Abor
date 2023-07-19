namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunitySchoolsRequest
    {
        public SchoolDistrict? SchoolDistrict { get; set; }
        public MiddleSchool? MiddleSchool { get; set; }
        public ElementarySchool? ElementarySchool { get; set; }
        public HighSchool? HighSchool { get; set; }
    }
}
