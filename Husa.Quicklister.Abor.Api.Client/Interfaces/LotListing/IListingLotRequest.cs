namespace Husa.Quicklister.Abor.Api.Client.Interfaces.LotListing
{
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest;
    using Husa.Quicklister.Extensions.Api.Client.Interfaces;

    public interface IListingLotRequest : ISaleListingRequest<ListingLotRequestQueryResponse, LotListingRequestDetailResponse>
    {
    }
}
