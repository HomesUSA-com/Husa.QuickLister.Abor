namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    public class ScrapedListingRequestFilter : BaseFilterRequest
    {
        public string BuilderName { get; set; }
    }
}
