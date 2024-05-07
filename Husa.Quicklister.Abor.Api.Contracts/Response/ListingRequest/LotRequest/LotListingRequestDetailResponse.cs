namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Response.LotListing;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;

    public class LotListingRequestDetailResponse : ListingRequestDetailResponse, IListingRequestDetailResponse
    {
        public Guid ListingId { get; set; }
        public string OwnerName { get; set; }
        public ListingStatusFieldsResponse StatusFieldsInfo { get; set; }
        public PublishInfoResponse PublishInfo { get; set; }
        public LotAddressResponse AddressInfo { get; set; }
        public LotSchoolsResponse SchoolsInfo { get; set; }
        public LotPropertyResponse PropertyInfo { get; set; }
        public LotFeaturesResponse FeaturesInfo { get; set; }
        public LotFinancialResponse FinancialInfo { get; set; }
        public LotShowingResponse ShowingInfo { get; set; }
    }
}
