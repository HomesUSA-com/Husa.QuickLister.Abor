namespace Husa.Quicklister.Abor.Domain.Enums
{
    public enum AlertType
    {
        PastDueEstimatedClosingDate,
        PastDueEstimatedCompletionDate,
        AgentBonusExpirationDate,
        AgentBonusExpirationDateOrLess,
        LockedListings,
        NotListedInMls,
        TempOffMarketBackOnMarket,
        TempOffMarketBackOnMarketDaysOrLess,
        EstimatedClosingDaysOrLess,
        CompletionDateDueDaysOrLess,
        InadequatePublicRemarks,
        ExpiringListings,
        CurrentDaysOnMarketOverDays,
        ActiveEmployees,
        PastDuePhotoRequests,
        CompletedHomesWithoutPhotoRequest,
        OrphanListings,
        ActiveAndPendingListing,
        ComparableAndRelistListing,
    }
}
