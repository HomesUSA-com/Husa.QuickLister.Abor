namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Extensions.Data.Queries.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;

    public class ListingSaleQueryDetailResult : ListingDetailsQueryResult, IProvideSaleListingUserInfo
    {
        public ListingSaleStatusFieldQueryResult StatusFieldsInfo { get; set; }

        public SalePropertyDetailQueryResult SaleProperty { get; set; }

        public ShowingTimeQueryResult ShowingTime { get; set; }
        public bool LockedByLegacy { get; set; }
        public Guid? UnlockedFromLegacyBy { get; set; }
        public string UnlockedFromLegacyByFullName { get; set; }
        public DateTime? UnlockedFromLegacyOn { get; set; }
    }
}
