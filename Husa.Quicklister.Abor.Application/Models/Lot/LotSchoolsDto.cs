namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotSchoolsDto
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }
    }
}
