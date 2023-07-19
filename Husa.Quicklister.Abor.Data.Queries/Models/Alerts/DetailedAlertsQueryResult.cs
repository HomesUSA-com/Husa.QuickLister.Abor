namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System.Collections.Generic;

    public class DetailedAlertsQueryResult
    {
        public IEnumerable<EstClosingDateQueryResult> PastDueEstimatedClosingDate { get; set; }
        public IEnumerable<EstClosingDateQueryResult> EstimatedClosingDaysOrLess { get; set; }
        public IEnumerable<CompletionDateQueryResult> PastDueEstimatedCompletionDate { get; set; }
        public IEnumerable<CompletionDateQueryResult> CompletionDateDueDaysOrLess { get; set; }
        public IEnumerable<AgentBonusExpDateQueryResult> AgentBonusExpirationDate { get; set; }
        public IEnumerable<AgentBonusExpDateQueryResult> AgentBonusExpirationDateOrLess { get; set; }
        public IEnumerable<TempOffMarketQueryResult> TempOffMarketBackOnMarket { get; set; }
        public IEnumerable<TempOffMarketQueryResult> TempOffMarketBackOnMarketDaysOrLess { get; set; }
        public IEnumerable<LockedListingsQueryResult> LockedListings { get; set; }
        public IEnumerable<ExpiringListingsQueryResult> ExpiringListings { get; set; }
        public IEnumerable<NotListedInMlsQueryResult> NotListedInMls { get; set; }
        public IEnumerable<CurrentDaysOnMarketOverQueryResult> CurrentDaysOnMarketOverDays { get; set; }
        public IEnumerable<InadequateRemarksQueryResult> InadequatePublicRemarks { get; set; }
        public IEnumerable<CompletedHomesWithoutPRQueryResult> CompletedHomesWithoutPhotoRequest { get; set; }
        public IEnumerable<CommunityMissingInfoQueryResult> CommunityProfilesMissingInformation { get; set; }
        public IEnumerable<MarketChangesQueryResult> MarketChangesSinceYesterday { get; set; }
        public IEnumerable<CommunityEmployeeQueryResult> ActiveEmployees { get; set; }
        public IEnumerable<PastDuePhotoRequestQueryResult> PastDuePhotoRequests { get; set; }
    }
}
