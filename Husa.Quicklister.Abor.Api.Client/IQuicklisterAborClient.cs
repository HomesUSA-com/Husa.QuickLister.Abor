namespace Husa.Quicklister.Abor.Api.Client
{
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Client.Interfaces.LotListing;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Client;
    using Husa.Quicklister.Extensions.Api.Client.LotListing;

    public interface IQuicklisterAborClient :
        IQuicklisterCommunityClient<ISaleCommunity>,
        IQuicklisterListingRequestClient<IListingSaleRequest, ListingSaleRequestQueryResponse, ListingSaleRequestDetailResponse>,
        IQuicklisterListingLotRequestClient<IListingLotRequest, ListingLotRequestQueryResponse, LotListingRequestDetailResponse>,
        IQuicklisterAlertClient<IAlert, AlertDetailResponse>
    {
        public ISaleListing SaleListing { get; }
        public IPlan Plan { get; }
        public IReport Report { get; }
        public void AddCustomHeader(string headerName, string headerValue);
    }
}
