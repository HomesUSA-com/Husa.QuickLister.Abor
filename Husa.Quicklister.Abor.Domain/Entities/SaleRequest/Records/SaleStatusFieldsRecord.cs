namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
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
            var summaryFields = SummaryExtensions.GetFieldSummary(this, oldStatusFielsValues, filterFields: GetFieldsByMlsStatus(mlsStatus));

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

        private static string[] GetFieldsByMlsStatus(MarketStatuses mlsStatus) => mlsStatus switch
        {
            MarketStatuses.Canceled => new string[] { nameof(CancelledReason) },
            MarketStatuses.Hold => new string[] { nameof(BackOnMarketDate), nameof(OffMarketDate) },
            MarketStatuses.Pending => new string[]
            {
                nameof(PendingDate),
                nameof(EstimatedClosedDate),
                nameof(HasBuyerAgent),
                nameof(AgentId),
                nameof(HasContingencyInfo),
            },
            MarketStatuses.ActiveUnderContract => new string[]
            {
                nameof(PendingDate),
                nameof(ClosedDate),
                nameof(EstimatedClosedDate),
                nameof(HasContingencyInfo),
                nameof(ContingencyInfo),
            },
            MarketStatuses.Closed => new string[]
            {
                nameof(HasContingencyInfo),
                nameof(AgentId),
                nameof(AgentIdSecond),
                nameof(HasSecondBuyerAgent),
                nameof(HasBuyerAgent),
                nameof(PendingDate),
                nameof(ClosedDate),
                nameof(ClosePrice),
                nameof(SaleTerms),
                nameof(SellConcess),
            },
            _ => Array.Empty<string>(),
        };
    }
}
