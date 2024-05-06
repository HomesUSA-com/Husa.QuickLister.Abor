namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Request.LotListing;

    public class LotListingRequestForUpdate : ListingRequest
    {
        public Guid ListingId { get; set; }
        public ListingStatusFieldsRequest StatusFieldsInfo { get; set; }
        public ListingPublishInfoRequest PublishInfo { get; set; }
        public AddressInfoRequest AddressInfo { get; set; }
        public LotPropertyRequest PropertyInfo { get; set; }
        public LotFeaturesRequest FeaturesInfo { get; set; }
        public LotFinancialRequest FinancialInfo { get; set; }
        public LotShowingRequest ShowingInfo { get; set; }
        public LotSchoolsRequest SchoolsInfo { get; set; }
    }
}
