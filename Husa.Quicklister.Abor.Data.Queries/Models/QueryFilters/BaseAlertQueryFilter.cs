namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    public class BaseAlertQueryFilter
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 10;

        public string SearchBy { get; set; }

        public bool IsOnlyCount { get; set; }

        public bool FillCommunityEmployees { get; set; }
    }
}
