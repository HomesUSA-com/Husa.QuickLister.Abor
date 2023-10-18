namespace Husa.Quicklister.Abor.Data.Documents.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using ExtensionInterfaces = Husa.Quicklister.Extensions.Data.Documents.Interfaces;

    public interface ISaleListingRequestQueriesRepository : ExtensionInterfaces.ISaleListingRequestQueriesRepository<SaleListingRequest, ListingSaleRequestQueryResult>
    {
        Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default);

        Task<DataSet<ListingSaleBillingQueryResult>> GetBillableListingsAsync(ListingSaleBillingQueryFilter queryFilter, CancellationToken cancellationToken = default);
    }
}
