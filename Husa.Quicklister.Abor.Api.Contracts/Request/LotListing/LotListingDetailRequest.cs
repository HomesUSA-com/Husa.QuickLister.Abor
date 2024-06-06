namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class LotListingDetailRequest : ListingRequest
    {
        public override ListType ListType => ListType.Lots;

        [Range(5000, 3000000, ErrorMessage = "{0} must be between {1} and {2}")]
        public override decimal? ListPrice { get; set; }
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
