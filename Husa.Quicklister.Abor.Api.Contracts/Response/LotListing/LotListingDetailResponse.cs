namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using System;

    public class LotListingDetailResponse : ListingResponse
    {
        public Guid? CompanyId { get; set; }

        public AddressInfoResponse AddressInfo { get; set; }
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
