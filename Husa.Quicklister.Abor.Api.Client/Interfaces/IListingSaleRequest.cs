namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Client.Interfaces;

    public interface IListingSaleRequest : ISaleListingRequest<ListingSaleRequestQueryResponse, ListingSaleRequestDetailResponse>
    {
    }
}
