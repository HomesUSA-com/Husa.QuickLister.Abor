namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public record LotSchoolRecord : IProvideSchool
    {
        [Required]
        public SchoolDistrict? SchoolDistrict { get; set; }

        [Required]
        public ElementarySchool? ElementarySchool { get; set; }

        [Required]
        public MiddleSchool? MiddleSchool { get; set; }

        [Required]
        public HighSchool? HighSchool { get; set; }

        public LotSchoolRecord CloneRecord() => (LotSchoolRecord)this.MemberwiseClone();
        public static LotSchoolRecord CreateRecord(LotSchoolsInfo schoolsInfo)
        {
            if (schoolsInfo == null)
            {
                return new();
            }

            return new()
            {
                SchoolDistrict = schoolsInfo.SchoolDistrict,
                ElementarySchool = schoolsInfo.ElementarySchool,
                MiddleSchool = schoolsInfo.MiddleSchool,
                HighSchool = schoolsInfo.HighSchool,
            };
        }
    }
}
