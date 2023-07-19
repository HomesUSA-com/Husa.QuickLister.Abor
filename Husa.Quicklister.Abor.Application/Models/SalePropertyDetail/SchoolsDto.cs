namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsDto
    {
        public SchoolDistrict? SchoolDistrict { get; set; }
        public MiddleSchool? MiddleSchool { get; set; }
        public ElementarySchool? ElementarySchool { get; set; }
        public HighSchool? HighSchool { get; set; }
    }
}
