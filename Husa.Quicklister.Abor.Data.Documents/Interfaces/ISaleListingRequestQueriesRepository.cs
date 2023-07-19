namespace Husa.Quicklister.Abor.Data.Documents.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Data.Documents.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface ISaleListingRequestQueriesRepository
    {
        Task<ListingRequestGridQueryResult<ListingSaleRequestQueryResult>> GetListingSaleRequestsAsync(ListingSaleRequestQueryFilter queryFilter, CancellationToken cancellationToken = default);

        Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default);

        Task<SaleListingRequest> GetListingRequestSaleAsyncOld(Guid requestId, CancellationToken cancellationToken = default);

        Task<IEnumerable<ListingSaleRequestQueryResult>> GetRequestsByListingSaleIdAsync(Guid listingSaleId, CancellationToken cancellationToken = default);

        Task<SaleListingRequest> GetListingSaleRequestByIdAndStatusAsync(Guid requestId, ListingRequestState requestState, CancellationToken cancellationToken = default);

        Task<SaleListingRequest> GetLastCompletedRequestAsync(Guid listingSaleId, DateTime? sysModifiedOn, CancellationToken cancellationToken = default);

        Task<SaleListingRequest> GetFirstCompletedRequestAsync(Guid listingSaleId, DateTime? sysModifiedOn, CancellationToken cancellationToken = default);

        Task<IEnumerable<SummarySectionQueryResult>> GetListingRequestSummaryAsync(Guid requestId, CancellationToken cancellationToken = default);

        Task<DataSet<ListingSaleBillingQueryResult>> GetBillableListingsAsync(ListingSaleBillingQueryFilter queryFilter, CancellationToken cancellationToken = default);
    }
}
