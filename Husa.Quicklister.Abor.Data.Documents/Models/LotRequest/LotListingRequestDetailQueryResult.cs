namespace Husa.Quicklister.Abor.Data.Documents.Models.LotRequest
{
    using System;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;

    public class LotListingRequestDetailQueryResult : ListingRequestDetailQueryResult
    {
        public Guid ListingId { get; set; }
        public string OwnerName { get; set; }
        public Guid? CommunityId { get; set; }
        public Guid CompanyId { get; set; }
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
