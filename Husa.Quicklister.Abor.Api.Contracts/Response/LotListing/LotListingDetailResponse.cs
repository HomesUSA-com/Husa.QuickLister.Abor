namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;

    public class LotListingDetailResponse : ListingResponse
    {
        public Guid? CompanyId { get; set; }

        public AddressInfoResponse AddressInfo { get; set; }
        public LotSchoolsResponse SchoolsInfo { get; set; }
        public LotPropertyResponse PropertyInfo { get; set; }
        public LotFeaturesResponse FeaturesInfo { get; set; }
        public LotFinancialResponse FinancialInfo { get; set; }
        public LotShowingResponse ShowingInfo { get; set; }
    }
}
