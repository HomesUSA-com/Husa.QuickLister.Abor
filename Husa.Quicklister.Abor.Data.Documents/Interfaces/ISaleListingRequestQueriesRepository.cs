namespace Husa.Quicklister.Abor.Data.Documents.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using ExtensionInterfaces = Husa.Quicklister.Extensions.Data.Documents.Interfaces;

    public interface ISaleListingRequestQueriesRepository : ExtensionInterfaces.ISaleListingRequestQueriesRepository<SaleListingRequest, ListingSaleRequestQueryResult>
    {
        Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}
