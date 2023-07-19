namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    public sealed class AlertTotalProjection
    {
        public AlertTotalProjection()
        {
            this.TempOffMarketBackOnMarketDaysOrLess = 0;
            this.TempOffMarketBackOnMarket = 0;
            this.ActiveAndPendingListings = 0;
            this.ComparableAndRelistListings = 0;
            this.ExpiringListings = 0;
            this.PastDueEstimatedClosingDate = 0;
            this.EstimatedClosingDaysOrLess = 0;
            this.PastDueEstimatedCompletionDate = 0;
            this.CompletionDateDueDaysOrLess = 0;
            this.AgentBonusExpirationDate = 0;
            this.LockedListings = 0;
            this.NotListedInMls = 0;
            this.AgentBonusExpirationDateOrLess = 0;
            this.CurrentDaysOnMarketOverDays = 0;
            this.InadequatePublicRemarks = 0;
            this.OrphanListings = 0;
        }

        public int TempOffMarketBackOnMarketDaysOrLess { get; set; }
        public int TempOffMarketBackOnMarket { get; set; }
        public int ActiveAndPendingListings { get; set; }
        public int ComparableAndRelistListings { get; set; }
        public int ExpiringListings { get; set; }
        public int PastDueEstimatedClosingDate { get; set; }
        public int EstimatedClosingDaysOrLess { get; set; }
        public int PastDueEstimatedCompletionDate { get; set; }
        public int CompletionDateDueDaysOrLess { get; set; }
        public int AgentBonusExpirationDate { get; set; }
        public int LockedListings { get; set; }
        public int NotListedInMls { get; set; }
        public int AgentBonusExpirationDateOrLess { get; set; }
        public int CurrentDaysOnMarketOverDays { get; set; }
        public int InadequatePublicRemarks { get; set; }
        public int OrphanListings { get; set; }

        public int Total => this.TempOffMarketBackOnMarketDaysOrLess +
            this.TempOffMarketBackOnMarket +
            this.ActiveAndPendingListings +
            this.ComparableAndRelistListings +
            this.ExpiringListings +
            this.PastDueEstimatedClosingDate +
            this.EstimatedClosingDaysOrLess +
            this.PastDueEstimatedCompletionDate +
            this.CompletionDateDueDaysOrLess +
            this.AgentBonusExpirationDate +
            this.LockedListings +
            this.NotListedInMls +
            this.AgentBonusExpirationDateOrLess +
            this.CurrentDaysOnMarketOverDays +
            this.InadequatePublicRemarks +
            this.OrphanListings;
    }
}
