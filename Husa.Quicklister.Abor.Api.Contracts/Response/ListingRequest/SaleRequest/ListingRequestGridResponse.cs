namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest
{
    using System.Collections.Generic;

    public class ListingRequestGridResponse<T>
        where T : class
    {
        public ListingRequestGridResponse(IEnumerable<T> data, int total, string continuationToken, string currentToken, string previousToken)
        {
            this.Data = data;
            this.Total = total;
            this.ContinuationToken = continuationToken;
            this.CurrentToken = currentToken;
            this.PreviousToken = previousToken;
        }

        public int Total { get; set; }

        public IEnumerable<T> Data { get; set; }

        public string ContinuationToken { get; set; }

        public string CurrentToken { get; set; }

        public string PreviousToken { get; set; }
    }
}
