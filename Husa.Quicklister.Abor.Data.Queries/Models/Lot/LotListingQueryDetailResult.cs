namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System;

    public class LotListingQueryDetailResult : ListingQueryResult
    {
        public Guid? CommunityId { get; set; }
        public Guid? CompanyId { get; set; }

        public AddressQueryResult AddressInfo { get; set; }
        public SchoolsQueryResult SchoolsInfo { get; set; }
        public LotPropertyQueryResult PropertyInfo { get; set; }
        public LotFeaturesQueryResult FeaturesInfo { get; set; }
        public LotFinancialQueryResult FinancialInfo { get; set; }
        public LotShowingQueryResult ShowingInfo { get; set; }
        public PublishInfoQueryResult PublishInfo { get; set; }
        public EmailLeadQueryResult EmailLead { get; set; }
        public ListingStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
