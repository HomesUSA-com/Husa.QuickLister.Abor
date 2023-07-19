namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSaleBillingQueryFilter : BaseQueryFilter
    {
        public ActionType? ActionType { get; set; }
        public string SearchBy { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
