namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideSchool
    {
        SchoolDistrict? SchoolDistrict { get; set; }

        ElementarySchool? ElementarySchool { get; set; }

        MiddleSchool? MiddleSchool { get; set; }

        HighSchool? HighSchool { get; set; }
    }
}
