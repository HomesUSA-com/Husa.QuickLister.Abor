namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public class LotListingDetailRequest : ListingRequest
    {
        public override ListType ListType => ListType.Lots;
        public string OwnerName { get; set; }
        public LotAdressRequest AddressInfo { get; set; }
        public LotPropertyRequest PropertyInfo { get; set; }
        public LotFeaturesRequest FeaturesInfo { get; set; }
        public LotFinancialRequest FinancialInfo { get; set; }
        public LotShowingRequest ShowingInfo { get; set; }
        public LotSchoolsRequest SchoolsInfo { get; set; }
        public ListingStatusFieldsRequest StatusFieldsInfo { get; set; }
    }
}
