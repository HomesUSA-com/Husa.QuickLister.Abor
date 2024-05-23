namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest
{
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest.SaleRequest;

    public class ListingSaleRequestQueryResponse : ListingRequestQueryResponse, ISaleListingRequestResponse
    {
        public bool EnableOpenHouse { get; set; }
    }
}
