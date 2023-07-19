namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Uploader;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response;

    public interface ISaleListing
    {
        Task<IEnumerable<Response.ListingSaleResponse>> GetAsync(Request.ListingSaleRequestFilter filters, CancellationToken token = default);

        Task<Response.ListingSaleDetailResponse> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<Response.ListingSaleDetailResponse> GetByAddressAsync(Request.ListingSaleFilterByAddress filter, CancellationToken token = default);

        Task UpdateListing(Guid id, Request.ListingSaleDetailRequest saleListingRequest, CancellationToken token = default);

        Task<Guid> CreateListing(Request.ListingSaleRequest listingSaleRequest, CancellationToken token = default);

        Task<CommandResult<ReverseProspectResponse>> GetReverseProspect(Guid listingId, MarketCode marketCode, CancellationToken token = default);

        Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken token = default);
    }
}
