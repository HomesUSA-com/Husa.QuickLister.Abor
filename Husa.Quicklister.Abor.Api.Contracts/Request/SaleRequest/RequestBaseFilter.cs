namespace Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class RequestBaseFilter
    {
        public ListingRequestState RequestState { get; set; } = ListingRequestState.Pending;

        public int? Take { get; set; } = 50;

        public string SortBy { get; set; } = "-syscreatedon";

        public string CurrentToken { get; set; }

        public string ContinuationToken { get; set; }
    }
}
