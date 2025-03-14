namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;

    public class ListingRequestBillingQueriesRepository : ListingBillingQueryRepository<SaleListingRequest>
    {
        public ListingRequestBillingQueriesRepository(
            IQueryListingBillingRepository billingRepository,
            IUserRepository userQueriesRepository,
            ISaleListingRequestQueriesRepository listingRequestRepository)
            : base(billingRepository, userQueriesRepository, listingRequestRepository)
        {
        }
    }
}
