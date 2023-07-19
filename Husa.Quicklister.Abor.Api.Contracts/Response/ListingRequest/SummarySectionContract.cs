namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest
{
    using System.Collections.Generic;

    public class SummarySectionContract
    {
        public string Name { get; set; }

        public IEnumerable<SummaryFieldContract> Fields { get; set; }
    }
}
