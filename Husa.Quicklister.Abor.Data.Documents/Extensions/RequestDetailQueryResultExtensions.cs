namespace Husa.Quicklister.Abor.Data.Documents.Extensions
{
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class RequestDetailQueryResultExtensions
    {
        public static void FillLockedInformation<TQueryResult, TListing>(this TQueryResult queryResult, TListing listing)
            where TQueryResult : ListingRequestDetailQueryResult
            where TListing : ListingDetailsQueryResult
        {
            if (listing.LockedStatus == LockedStatus.LockedBySystem)
            {
                queryResult.LockedByUsername = UserConstants.LockedBySystemLabel;
            }
            else
            {
                queryResult.LockedBy = listing.LockedBy;
                queryResult.LockedByUsername = listing.LockedByUsername;
            }

            queryResult.LockedStatus = listing.LockedStatus;
            queryResult.LockedOn = listing.LockedOn;
            if (queryResult.PublishInfo.PublishType is null)
            {
                queryResult.PublishInfo.PublishType = listing.PublishInfo.PublishType;
            }
        }
    }
}
