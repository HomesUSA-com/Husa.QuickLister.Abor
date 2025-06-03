namespace Husa.Quicklister.Abor.Domain.Entities.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.IdentityModel.Tokens;

    public record StatusFieldsRecord : IProvideStatusFields
    {
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
        public bool HasContingencyInfo { get; set; }
        public ICollection<SaleTerms> SaleTerms { get; set; }
        public string SellConcess { get; set; }
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public static TResult CreateRecord<T, TResult>(T statusFieldInfo)
            where T : ListingStatusFieldsInfo
            where TResult : StatusFieldsRecord, new()
            => new()
            {
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
                HasContingencyInfo = statusFieldInfo.HasContingencyInfo,
                SaleTerms = statusFieldInfo.SaleTerms,
                SellConcess = statusFieldInfo.SellConcess,
                ContingencyInfo = statusFieldInfo.ContingencyInfo,
            };

        public virtual void UpdateInformation<T>(T statusFieldInfo)
            where T : ListingStatusFieldsInfo
        {
            ArgumentNullException.ThrowIfNull(statusFieldInfo);

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
            this.HasContingencyInfo = statusFieldInfo.HasContingencyInfo;
            this.SaleTerms = statusFieldInfo.SaleTerms;
            this.SellConcess = statusFieldInfo.SellConcess;
            this.ContingencyInfo = statusFieldInfo.ContingencyInfo;
        }

        public virtual SummarySection GetSummarySection<T>(T oldStatusFielsValues, MarketStatuses? mlsStatus, string sectionName)
            where T : StatusFieldsRecord
        {
            var summaryFields = this.GetFieldSummary(oldStatusFielsValues, mlsStatus);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = sectionName,
                Fields = summaryFields,
            };
        }

        protected IEnumerable<SummaryField> GetFieldSummary<T>(T oldStatusFielsValues, MarketStatuses? mlsStatus)
            where T : StatusFieldsRecord
        {
            var fieldsMlsStatus = this.GetFieldsByMlsStatus(mlsStatus);
            if (fieldsMlsStatus.IsNullOrEmpty())
            {
                return Array.Empty<SummaryField>();
            }

            return SummaryExtensions.GetFieldSummary(this, oldStatusFielsValues, filterFields: fieldsMlsStatus);
        }

        protected virtual string[] GetFieldsByMlsStatus(MarketStatuses? mlsStatus) => mlsStatus switch
        {
            MarketStatuses.Canceled => new string[] { nameof(this.CancelledReason) },
            MarketStatuses.Hold => new string[] { nameof(this.BackOnMarketDate), nameof(this.OffMarketDate) },
            MarketStatuses.Pending => new string[]
            {
                nameof(this.PendingDate),
                nameof(this.EstimatedClosedDate),
                nameof(this.HasBuyerAgent),
                nameof(this.AgentId),
                nameof(this.HasContingencyInfo),
            },
            MarketStatuses.ActiveUnderContract => new string[]
            {
                nameof(this.PendingDate),
                nameof(this.ClosedDate),
                nameof(this.EstimatedClosedDate),
                nameof(this.HasContingencyInfo),
                nameof(this.ContingencyInfo),
            },
            MarketStatuses.Closed => new string[]
            {
                nameof(this.HasContingencyInfo),
                nameof(this.AgentId),
                nameof(this.AgentIdSecond),
                nameof(this.HasSecondBuyerAgent),
                nameof(this.HasBuyerAgent),
                nameof(this.PendingDate),
                nameof(this.ClosedDate),
                nameof(this.ClosePrice),
                nameof(this.SaleTerms),
                nameof(this.SellConcess),
            },
            _ => Array.Empty<string>(),
        };
    }
}
