namespace Husa.Quicklister.Abor.Data.Documents.QueryFilters
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class RequestBaseQueryFilter
    {
        public ListingRequestState RequestState { get; set; }

        public int? Take { get; set; }

        public string SortBy { get; set; }

        public string CurrentToken { get; set; }

        public string ContinuationToken { get; set; }
    }
}
