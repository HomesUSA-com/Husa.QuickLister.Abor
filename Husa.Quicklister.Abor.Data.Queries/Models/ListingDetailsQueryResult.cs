namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using Husa.Quicklister.Extensions.Data.Queries.Models;

    public class ListingDetailsQueryResult : ListingQueryResult
    {
        public PublishInfoQueryResult PublishInfo { get; set; }

        public EmailLeadQueryResult EmailLead { get; set; }
    }
}
