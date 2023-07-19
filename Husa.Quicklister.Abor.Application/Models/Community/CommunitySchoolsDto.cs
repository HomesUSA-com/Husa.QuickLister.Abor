namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunitySchoolsDto
    {
        public SchoolDistrict? SchoolDistrict { get; set; }
        public MiddleSchool? MiddleSchool { get; set; }
        public ElementarySchool? ElementarySchool { get; set; }
        public HighSchool? HighSchool { get; set; }
    }
}
