namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using Husa.Quicklister.Extensions.Api.Contracts.Response;

    public class LotListingDetailResponse : ListingResponse
    {
        public LotAddressResponse AddressInfo { get; set; }
        public LotSchoolsResponse SchoolsInfo { get; set; }
        public LotPropertyResponse PropertyInfo { get; set; }
        public LotFeaturesResponse FeaturesInfo { get; set; }
        public LotFinancialResponse FinancialInfo { get; set; }
        public LotShowingResponse ShowingInfo { get; set; }
        public PublishInfoResponse PublishInfo { get; set; }
        public EmailLeadResponse EmailLead { get; set; }
        public ListingStatusFieldsResponse StatusFieldsInfo { get; set; }
    }
}
