namespace Husa.Quicklister.Abor.Domain.Extensions.Listing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;

    public static class MarketStatusesExtensions
    {
        public static readonly IEnumerable<MarketStatuses> ActiveListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
        };
        public static readonly IEnumerable<MarketStatuses> OrphanListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
            MarketStatuses.Closed,
        };
        public static readonly IEnumerable<MarketStatuses> PendingListingStatuses = new[] { MarketStatuses.Pending };
        public static readonly IEnumerable<MarketStatuses> PendingAndCanceledStatuses = new[]
        {
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
        };
        public static readonly IEnumerable<MarketStatuses> ActivePhotoRequestListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.Hold,
        };
        public static readonly IEnumerable<MarketStatuses> ActiveAndPendingListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
        };
        public static readonly IEnumerable<MarketStatuses> ExistingListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
        };
        public static readonly IEnumerable<MarketStatuses> PendingAndActiveUnderContractStatuses = new[]
        {
            MarketStatuses.Pending,
            MarketStatuses.ActiveUnderContract,
        };
    }
}
