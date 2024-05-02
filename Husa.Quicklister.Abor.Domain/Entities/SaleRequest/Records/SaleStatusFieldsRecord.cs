namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record SaleStatusFieldsRecord : StatusFieldsRecord, IProvideSummary, IProvideSaleStatusFields
    {
        public const string SummarySection = "Status Fields";

        public bool HasContingencyInfo { get; set; }
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }
        public ICollection<SaleTerms> SaleTerms { get; set; }
        public string SellConcess { get; set; }

        public SaleStatusFieldsRecord CloneRecord() => (SaleStatusFieldsRecord)this.MemberwiseClone();

        public static SaleStatusFieldsRecord CreateRecord(ListingSaleStatusFieldsInfo statusFieldInfo)
        {
            var record = StatusFieldsRecord.CreateRecord<ListingSaleStatusFieldsInfo, SaleStatusFieldsRecord>(statusFieldInfo);
            record.HasContingencyInfo = statusFieldInfo.HasContingencyInfo;
            record.ContingencyInfo = statusFieldInfo.ContingencyInfo;
            record.SaleTerms = statusFieldInfo.SaleTerms;
            record.SellConcess = statusFieldInfo.SellConcess;
            return record;
        }

        public virtual void UpdateInformation(ListingSaleStatusFieldsInfo statusFieldInfo)
        {
            ArgumentNullException.ThrowIfNull(statusFieldInfo);

            this.UpdateInformation<ListingSaleStatusFieldsInfo>(statusFieldInfo);

            this.HasContingencyInfo = statusFieldInfo.HasContingencyInfo;
            this.ContingencyInfo = statusFieldInfo.ContingencyInfo;
            this.SaleTerms = statusFieldInfo.SaleTerms;
            this.SellConcess = statusFieldInfo.SellConcess;
        }

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
