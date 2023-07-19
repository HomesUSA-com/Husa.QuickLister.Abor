namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSaleBillingRequestFilter : BaseFilterRequest
    {
        public ActionType? ActionType { get; set; }
        public string SearchBy { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
