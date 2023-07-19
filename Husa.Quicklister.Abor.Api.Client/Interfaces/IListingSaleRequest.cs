namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;

    public interface IListingSaleRequest
    {
        Task<ListingRequestGridResponse<ListingSaleRequestQueryResponse>> GetListRequestAsync(ListingSaleRequestFilter requestFilter, CancellationToken token = default);

        Task<ListingSaleRequestDetailResponse> GetListRequestSaleByIdAsync(Guid id, CancellationToken token = default);

        Task DeleteRequestAsync(Guid id, CancellationToken token = default);

        Task ReturnRequestAsync(Guid id, CancellationToken token = default);

        Task ApproveRequestAsync(Guid id, CancellationToken token = default);

        Task ProcessRequestAsync(Guid id, CancellationToken token = default);

        Task CompleteRequestAsync(Guid id, string mlsNumber, CancellationToken token = default);

        Task<IEnumerable<ListingSaleRequestQueryResponse>> GetRequestByListingSaleIdAsync(Guid listingSaleId, CancellationToken token = default);

        Task<IEnumerable<SummarySectionContract>> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<object>> CreateAsync(Guid listingSaleId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Guid>> CreateRequestsByCommunityAsync(Guid communityId, CancellationToken cancellationToken = default);
    }
}
