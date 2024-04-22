namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class LotSchoolsInfo : ValueObject, IProvideSchool
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }

        public virtual LotSchoolsInfo Clone()
        {
            return (LotSchoolsInfo)this.MemberwiseClone();
        }

        public LotSchoolsInfo ImportSchools(SchoolsInfo schools)
        {
            var clonnedSchools = this.Clone();
            clonnedSchools.SchoolDistrict = schools.SchoolDistrict;
            clonnedSchools.MiddleSchool = schools.MiddleSchool;
            clonnedSchools.ElementarySchool = schools.ElementarySchool;
            clonnedSchools.HighSchool = schools.HighSchool;
            return clonnedSchools;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.SchoolDistrict;
            yield return this.MiddleSchool;
            yield return this.ElementarySchool;
            yield return this.HighSchool;
        }
    }
}
