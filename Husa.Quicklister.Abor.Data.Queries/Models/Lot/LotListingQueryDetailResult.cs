namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System;

    public class LotListingQueryDetailResult : ListingDetailsQueryResult
    {
        public Guid? CommunityId { get; set; }
        public int? AlsoListedAs { get; set; }

        public bool BuilderRestrictions { get; set; }

        public LotAddressQueryResult AddressInfo { get; set; }
        public SchoolsInfoQueryResult SchoolsInfo { get; set; }
        public LotPropertyQueryResult PropertyInfo { get; set; }
        public LotFeaturesQueryResult FeaturesInfo { get; set; }
        public LotFinancialQueryResult FinancialInfo { get; set; }
        public LotShowingQueryResult ShowingInfo { get; set; }
        public ListingStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
