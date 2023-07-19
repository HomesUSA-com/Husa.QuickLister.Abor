namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts.Listing
{
    public class ListingSaleAlertsQueryResult
    {
        public EstClosingDateQueryResult PastDueEstimatedClosingDate { get; set; }
        public EstClosingDateQueryResult EstimatedClosingDaysOrLess { get; set; }
        public CompletionDateQueryResult PastDueEstimatedCompletionDate { get; set; }
        public CompletionDateQueryResult CompletionDateDueDaysOrLess { get; set; }
        public AgentBonusExpDateQueryResult AgentBonusExpirationDate { get; set; }
        public AgentBonusExpDateQueryResult AgentBonusExpirationDateOrLess { get; set; }
        public TempOffMarketQueryResult TempOffMarketBackOnMarket { get; set; }
        public TempOffMarketQueryResult TempOffMarketBackOnMarketDaysOrLess { get; set; }
        public LockedListingsQueryResult LockedListings { get; set; }
        public ExpiringListingsQueryResult ExpiringListings { get; set; }
        public NotListedInMlsQueryResult NotListedInMls { get; set; }
        public CurrentDaysOnMarketOverQueryResult CurrentDaysOnMarketOverDays { get; set; }
        public InadequateRemarksQueryResult InadequatePublicRemarks { get; set; }
        public CompletedHomesWithoutPRQueryResult CompletedHomesWithoutPhotoRequest { get; set; }
        public CommunityMissingInfoQueryResult CommunityProfilesMissingInformation { get; set; }
        public ListingSaleQueryResult MarketChangesSinceYesterday { get; set; }
    }
}
