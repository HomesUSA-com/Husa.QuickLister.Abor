namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotSchools
    {
        SchoolDistrict? SchoolDistrict { get; set; }

        ElementarySchool? ElementarySchool { get; set; }

        MiddleSchool? MiddleSchool { get; set; }

        HighSchool? HighSchool { get; set; }
    }
}
