namespace Husa.Quicklister.Abor.Data.Documents.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.LotRequest;
    using ExtensionInterfaces = Husa.Quicklister.Extensions.Data.Documents.Interfaces;

    public interface ILotListingRequestQueriesRepository : ExtensionInterfaces.ILotListingRequestQueriesRepository<ListingRequestQueryResult>
    {
        Task<LotListingRequestDetailQueryResult> GetRequestByIdAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}
