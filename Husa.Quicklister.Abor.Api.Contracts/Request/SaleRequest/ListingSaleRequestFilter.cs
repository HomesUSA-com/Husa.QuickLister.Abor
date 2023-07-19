namespace Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest
{
    using System;

    public class ListingSaleRequestFilter : RequestBaseFilter
    {
        public Guid CompanyId { get; set; }

        public string SearchFilter { get; set; } = string.Empty;

        public bool IsPrint { get; set; } = false;

        public Guid? SaleListingId { get; set; }
    }
}
