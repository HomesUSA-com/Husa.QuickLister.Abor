namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record SchoolRecord : IProvideSummary
    {
        public const string SummarySection = "Schools";

        [Required]
        public SchoolDistrict SchoolDistrict { get; set; }

        [Required]
        public ElementarySchool ElementarySchool { get; set; }

        [Required]
        public MiddleSchool MiddleSchool { get; set; }

        [Required]
        public HighSchool HighSchool { get; set; }

        public SchoolRecord CloneRecord() => (SchoolRecord)this.MemberwiseClone();
        public static SchoolRecord CreateRecord(SchoolsInfo schoolsInfo)
        {
            if (schoolsInfo == null)
            {
                return new();
            }

            return new()
            {
                SchoolDistrict = schoolsInfo.SchoolDistrict ?? throw new DomainException(nameof(schoolsInfo.SchoolDistrict)),
                ElementarySchool = schoolsInfo.ElementarySchool ?? throw new DomainException(nameof(schoolsInfo.ElementarySchool)),
                MiddleSchool = schoolsInfo.MiddleSchool ?? throw new DomainException(nameof(schoolsInfo.MiddleSchool)),
                HighSchool = schoolsInfo.HighSchool ?? throw new DomainException(nameof(schoolsInfo.HighSchool)),
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
