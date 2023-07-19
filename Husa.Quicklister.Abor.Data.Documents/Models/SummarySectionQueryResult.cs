namespace Husa.Quicklister.Abor.Data.Documents.Models
{
    using System.Collections.Generic;

    public class SummarySectionQueryResult
    {
        public string Name { get; set; }

        public IEnumerable<SummaryFieldQueryResult> Fields { get; set; }
    }
}
