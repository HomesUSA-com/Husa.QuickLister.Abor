namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record StatusFieldsRecord : IProvideSummary, IProvideStatusFields
    {
        public const string SummarySection = "Status Fields";

        public string ContingencyInfo { get; set; }

        public string SaleTerms2nd { get; set; }

        public DateTime? ContractDate { get; set; }

        public DateTime? ExpiredDateOption { get; set; }

        public string KickOutInformation { get; set; }

        public HowSold? HowSold { get; set; }

        public decimal SellPoints { get; set; }

        public string SellConcess { get; set; }

        public ICollection<SellerConcessionDescription> SellerConcessionDescription { get; set; }

        public DateTime? CancelDate { get; set; }

        public CancelledOptions? CancelledOption { get; set; }

        public string CancelledReason { get; set; }

        public decimal? ClosePrice { get; set; }

        public DateTime? EstimatedClosedDate { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent { get; set; }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? PendingDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public StatusFieldsRecord CloneRecord() => (StatusFieldsRecord)this.MemberwiseClone();

        public static StatusFieldsRecord CreateRecord(ListingSaleStatusFieldsInfo statusFieldInfo) => new()
        {
            ContingencyInfo = statusFieldInfo.ContingencyInfo,
            SaleTerms2nd = statusFieldInfo.SaleTerms2nd,
            ContractDate = statusFieldInfo.ContractDate,
            ExpiredDateOption = statusFieldInfo.ExpiredDateOption,
            KickOutInformation = statusFieldInfo.KickOutInformation,
            HowSold = statusFieldInfo.HowSold,
            SellPoints = statusFieldInfo.SellPoints,
            SellConcess = statusFieldInfo.SellConcess,
            SellerConcessionDescription = statusFieldInfo.SellerConcessionDescription,
            CancelDate = statusFieldInfo.CancelDate,
            CancelledOption = statusFieldInfo.CancelledOption,
            CancelledReason = statusFieldInfo.CancelledReason,
            ClosePrice = statusFieldInfo.ClosePrice,
            EstimatedClosedDate = statusFieldInfo.EstimatedClosedDate,
            AgentId = statusFieldInfo.AgentId,
            HasBuyerAgent = statusFieldInfo.HasBuyerAgent,
            BackOnMarketDate = statusFieldInfo.BackOnMarketDate,
            PendingDate = statusFieldInfo.PendingDate,
            ClosedDate = statusFieldInfo.ClosedDate,
            OffMarketDate = statusFieldInfo.OffMarketDate,
        };

        public virtual void UpdateInformation(ListingSaleStatusFieldsInfo statusFieldInfo)
        {
            if (statusFieldInfo is null)
            {
                throw new ArgumentNullException(nameof(statusFieldInfo));
            }

            this.ContingencyInfo = statusFieldInfo.ContingencyInfo;
            this.SaleTerms2nd = statusFieldInfo.SaleTerms2nd;
            this.ContractDate = statusFieldInfo.ContractDate;
            this.ExpiredDateOption = statusFieldInfo.ExpiredDateOption;
            this.KickOutInformation = statusFieldInfo.KickOutInformation;
            this.HowSold = statusFieldInfo.HowSold;
            this.SellPoints = statusFieldInfo.SellPoints;
            this.SellConcess = statusFieldInfo.SellConcess;
            this.SellerConcessionDescription = statusFieldInfo.SellerConcessionDescription;
            this.CancelDate = statusFieldInfo.CancelDate;
            this.CancelledOption = statusFieldInfo.CancelledOption;
            this.CancelledReason = statusFieldInfo.CancelledReason;
            this.ClosePrice = statusFieldInfo.ClosePrice;
            this.EstimatedClosedDate = statusFieldInfo.EstimatedClosedDate;
            this.AgentId = statusFieldInfo.AgentId;
            this.HasBuyerAgent = statusFieldInfo.HasBuyerAgent;
            this.BackOnMarketDate = statusFieldInfo.BackOnMarketDate;
            this.PendingDate = statusFieldInfo.PendingDate;
            this.ClosedDate = statusFieldInfo.ClosedDate;
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
            MarketStatuses.Cancelled => new string[] { nameof(CancelledOption), nameof(CancelledReason) },
            MarketStatuses.Pending => new string[] { nameof(ContractDate), nameof(EstimatedClosedDate), nameof(HasBuyerAgent), nameof(AgentId) },
            MarketStatuses.PendingSB => new string[] { nameof(ContractDate), nameof(EstimatedClosedDate), nameof(HasBuyerAgent), nameof(AgentId) },
            MarketStatuses.Withdrawn => new string[] { nameof(OffMarketDate), nameof(BackOnMarketDate) },
            MarketStatuses.Sold => new string[]
            {
                    nameof(HowSold),
                    nameof(ContractDate),
                    nameof(ClosedDate),
                    nameof(ClosePrice),
                    nameof(ContingencyInfo),
                    nameof(SaleTerms2nd),
                    nameof(SellConcess),
                    nameof(SellPoints),
                    nameof(SellerConcessionDescription),
                    nameof(HasBuyerAgent),
                    nameof(AgentId),
            },
            _ => Array.Empty<string>(),
        };
    }
}
