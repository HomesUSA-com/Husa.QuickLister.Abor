namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    public class BaseQueryFilter
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 50;

        public string SortBy { get; set; }

        public bool IsOnlyCount { get; set; }
    }
}
