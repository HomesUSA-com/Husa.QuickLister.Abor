namespace Husa.Quicklister.Abor.Domain.Entities.Request
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record SalesOfficeRecord : IProvideSummary
    {
        public const string SummarySection = "SalesOffice";

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string StreetSuffix { get; set; }

        public Cities? SalesOfficeCity { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "The {0} value must be {1} of length")]
        public string SalesOfficeZip { get; set; }

        public SalesOfficeRecord CloneRecord() => (SalesOfficeRecord)this.MemberwiseClone();

        public static SalesOfficeRecord CreateRecord(SalesOffice saleOfficeInfo)
        {
            if (saleOfficeInfo == null)
            {
                return new();
            }

            return new()
            {
                StreetNumber = saleOfficeInfo.StreetNumber,
                StreetName = saleOfficeInfo.StreetName,
                StreetSuffix = saleOfficeInfo.StreetSuffix,
                SalesOfficeCity = saleOfficeInfo.SalesOfficeCity,
                SalesOfficeZip = saleOfficeInfo.SalesOfficeZip,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
