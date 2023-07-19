namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System.ComponentModel.DataAnnotations;

    public class BaseFilterRequest
    {
        public int Skip { get; set; } = 0;

        [Range(1, 300)]
        public int Take { get; set; } = 100;

        public string SortBy { get; set; }

        public bool IsOnlyCount { get; set; } = false;
    }
}
