namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;
    using Husa.Xml.Domain.Enums;

    public class SchoolsInfo : ValueObject
    {
        public SchoolDistrict? SchoolDistrict { get; set; }

        public ElementarySchool? ElementarySchool { get; set; }

        public MiddleSchool? MiddleSchool { get; set; }

        public HighSchool? HighSchool { get; set; }

        public static SchoolsInfo ImportFromXml(SubdivisionResponse subdivision, SchoolsInfo schoolsInfo)
        {
            var importedSchools = new SchoolsInfo();
            if (schoolsInfo != null)
            {
                importedSchools = schoolsInfo.Clone();
            }

            if (!subdivision.SchoolDistrict.Any())
            {
                return importedSchools;
            }

            var district = subdivision.SchoolDistrict.FirstOrDefault();
            if (district == null)
            {
                return importedSchools;
            }

            if (!string.IsNullOrEmpty(district.Name))
            {
                var cleanedDistrict = district.Name.Replace(" ISD", string.Empty).Trim();
                importedSchools.SchoolDistrict = cleanedDistrict.GetEnumFromText<SchoolDistrict>();
            }

            if (district.School.Any())
            {
                var elementarySchool = district.School.FirstOrDefault(s => s.Type == SchoolType.Elementary);
                if (elementarySchool != null)
                {
                    importedSchools.ElementarySchool = elementarySchool.Name.GetEnumFromText<ElementarySchool>();
                }

                var middleSchool = district.School.FirstOrDefault(s => s.Type == SchoolType.Middle);
                if (middleSchool != null)
                {
                    importedSchools.MiddleSchool = middleSchool.Name.GetEnumFromText<MiddleSchool>();
                }

                var highSchool = district.School.FirstOrDefault(s => s.Type == SchoolType.High);
                if (highSchool != null)
                {
                    importedSchools.HighSchool = highSchool.Name.GetEnumFromText<HighSchool>();
                }
            }

            return importedSchools;
        }

        public SchoolsInfo Clone()
        {
            return (SchoolsInfo)this.MemberwiseClone();
        }

        public SchoolsInfo ImportSchools(SchoolsInfo schools)
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
