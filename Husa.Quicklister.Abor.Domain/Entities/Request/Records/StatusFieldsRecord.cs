namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record StatusFieldsRecord : IProvideSummary, IProvideStatusFields, IProvideSaleStatusFields
    {
        public const string SummarySection = "Status Fields";

        public bool HasContingencyInfo { get; set; }
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }
        public ICollection<SaleTerms> SaleTerms { get; set; }
        public string SellConcess { get; set; }
        public DateTime? PendingDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? EstimatedClosedDate { get; set; }
        public string CancelledReason { get; set; }
        public decimal? ClosePrice { get; set; }
        public Guid? AgentId { get; set; }
        public bool HasBuyerAgent { get; set; }
        public bool HasSecondBuyerAgent { get; set; }
        public Guid? AgentIdSecond { get; set; }
        public DateTime? BackOnMarketDate { get; set; }
        public DateTime? OffMarketDate { get; set; }

        public StatusFieldsRecord CloneRecord() => (StatusFieldsRecord)this.MemberwiseClone();

        public static StatusFieldsRecord CreateRecord(ListingSaleStatusFieldsInfo statusFieldInfo) => new()
        {
            HasContingencyInfo = statusFieldInfo.HasContingencyInfo,
            ContingencyInfo = statusFieldInfo.ContingencyInfo,
            SaleTerms = statusFieldInfo.SaleTerms,
            SellConcess = statusFieldInfo.SellConcess,
            PendingDate = statusFieldInfo.PendingDate,
            ClosedDate = statusFieldInfo.ClosedDate,
            EstimatedClosedDate = statusFieldInfo.EstimatedClosedDate,
            CancelledReason = statusFieldInfo.CancelledReason,
            ClosePrice = statusFieldInfo.ClosePrice,
            AgentId = statusFieldInfo.AgentId,
            HasBuyerAgent = statusFieldInfo.HasBuyerAgent,
            HasSecondBuyerAgent = statusFieldInfo.HasSecondBuyerAgent,
            AgentIdSecond = statusFieldInfo.AgentIdSecond,
            BackOnMarketDate = statusFieldInfo.BackOnMarketDate,
            OffMarketDate = statusFieldInfo.OffMarketDate,
        };

        public virtual void UpdateInformation(ListingSaleStatusFieldsInfo statusFieldInfo)
        {
            if (statusFieldInfo is null)
            {
                throw new ArgumentNullException(nameof(statusFieldInfo));
            }

            this.HasContingencyInfo = statusFieldInfo.HasContingencyInfo;
            this.ContingencyInfo = statusFieldInfo.ContingencyInfo;
            this.SaleTerms = statusFieldInfo.SaleTerms;
            this.SellConcess = statusFieldInfo.SellConcess;
            this.PendingDate = statusFieldInfo.PendingDate;
            this.ClosedDate = statusFieldInfo.ClosedDate;
            this.EstimatedClosedDate = statusFieldInfo.EstimatedClosedDate;
            this.CancelledReason = statusFieldInfo.CancelledReason;
            this.ClosePrice = statusFieldInfo.ClosePrice;
            this.AgentId = statusFieldInfo.AgentId;
            this.HasBuyerAgent = statusFieldInfo.HasBuyerAgent;
            this.HasSecondBuyerAgent = statusFieldInfo.HasSecondBuyerAgent;
            this.AgentIdSecond = statusFieldInfo.AgentIdSecond;
            this.BackOnMarketDate = statusFieldInfo.BackOnMarketDate;
            this.OffMarketDate = statusFieldInfo.OffMarketDate;
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

        public virtual SummarySection GetSummary(StatusFieldsRecord oldStatusFielsValues, MarketStatuses mlsStatus)
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
