namespace Husa.Quicklister.Abor.Api.Contracts.Request.Reports
{
    using System;
    using System.Collections.Generic;

    public class InvoiceRequest
    {
        public Guid CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<Guid> ListingIds { get; set; }
    }
}
