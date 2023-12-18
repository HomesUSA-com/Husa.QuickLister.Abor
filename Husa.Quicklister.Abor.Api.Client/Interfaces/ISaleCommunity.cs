namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityExtension = Husa.Quicklister.Extensions.Api.Client.Interfaces.ISaleCommunity;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response.Community;

    public interface ISaleCommunity : CommunityExtension
    {
        Task<Guid> CreateCommunity(Request.CreateCommunityRequest communityRequest, CancellationToken token = default);

        Task<IEnumerable<Response.CommunityDataQueryResponse>> GetAsync(Request.CommunityRequestFilter filter, CancellationToken token = default);

        Task<Response.CommunitySaleResponse> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<Response.CommunitySaleResponse> GetByNameAsync(Request.CommunityByNameFilter filter, CancellationToken token = default);

        Task UpdateCommunity(Guid id, Request.CommunitySaleRequest communitySaleRequest, CancellationToken token = default);

        Task<Response.CommunitySaleResponse> GetCommunityWithListingProjection(Guid communityId, Guid listingId, CancellationToken token = default);

        Task ApproveCommunity(Guid id, CancellationToken token = default);
    }
}
