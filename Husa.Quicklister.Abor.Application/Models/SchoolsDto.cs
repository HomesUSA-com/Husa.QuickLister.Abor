namespace Husa.Quicklister.Abor.Application.Models
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsDto
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public OtherElementarySchool? OtherElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public OtherMiddleSchool? OtherMiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }

        public OtherHighSchool? OtherHighSchool { get; set; }
    }
}
