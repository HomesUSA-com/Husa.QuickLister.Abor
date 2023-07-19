namespace Husa.Quicklister.Abor.Data.Documents.Models
{
    using System.Collections.Generic;

    public class ListingRequestGridQueryResult<T>
        where T : class
    {
        public ListingRequestGridQueryResult(IEnumerable<T> data, string continuationToken)
        {
            this.Data = data;
            this.ContinuationToken = continuationToken;
        }

        public int Total { get; set; }

        public IEnumerable<T> Data { get; set; }

        public string ContinuationToken { get; set; }
    }
}
