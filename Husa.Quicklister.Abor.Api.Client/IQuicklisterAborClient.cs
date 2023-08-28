namespace Husa.Quicklister.Abor.Api.Client
{
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Client;

    public interface IQuicklisterAborClient : IQuicklisterClient<IListingSaleRequest, ListingSaleRequestQueryResponse, ListingSaleRequestDetailResponse>
    {
        public ISaleListing SaleListing { get; }
        public ISaleCommunity SaleCommunity { get; }
        public IPlan Plan { get; }
        public IAlert Alert { get; }
        public IReport Report { get; }
    }
}
