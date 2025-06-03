namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record SaleStatusFieldsRecord : StatusFieldsRecord, IProvideSummary
    {
        public const string SummarySection = "Status Fields";

        public SaleStatusFieldsRecord CloneRecord() => (SaleStatusFieldsRecord)this.MemberwiseClone();

        public static SaleStatusFieldsRecord CreateRecord(ListingStatusFieldsInfo statusFieldInfo)
            => StatusFieldsRecord.CreateRecord<ListingStatusFieldsInfo, SaleStatusFieldsRecord>(statusFieldInfo);

        public virtual void UpdateInformation(ListingStatusFieldsInfo statusFieldInfo)
            => this.UpdateInformation<ListingStatusFieldsInfo>(statusFieldInfo);

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity);

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

        public virtual SummarySection GetSummary(SaleStatusFieldsRecord oldStatusFielsValues, MarketStatuses mlsStatus)
        {
            var summaryFields = this.GetFieldSummary(oldStatusFielsValues, mlsStatus);

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
