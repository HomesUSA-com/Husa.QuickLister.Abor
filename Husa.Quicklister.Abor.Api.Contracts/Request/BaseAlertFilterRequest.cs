namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System.ComponentModel.DataAnnotations;

    public class BaseAlertFilterRequest
    {
        public int Skip { get; set; } = 0;

        [Range(1, 20)]
        public int Take { get; set; } = 10;

        public string SearchBy { get; set; }

        public bool IsOnlyCount { get; set; }

        public bool FillCommunityEmployees { get; set; }
    }
}
