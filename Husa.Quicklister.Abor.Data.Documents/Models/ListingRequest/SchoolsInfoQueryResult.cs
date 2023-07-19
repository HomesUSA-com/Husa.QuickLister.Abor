namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SchoolsInfoQueryResult
    {
        public SchoolDistrict SchoolDistrict { get; set; }

        public ElementarySchool ElementarySchool { get; set; }

        public MiddleSchool MiddleSchool { get; set; }

        public HighSchool HighSchool { get; set; }
    }
}
