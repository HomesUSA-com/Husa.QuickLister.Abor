namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingRequestStatusFieldsQueryResult : ListingStatusFieldsQueryResult
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }
    }
}
