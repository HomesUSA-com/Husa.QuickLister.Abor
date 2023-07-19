namespace Husa.Quicklister.Abor.Data.Documents.QueryFilters
{
    using System;

    public class ListingSaleRequestQueryFilter : RequestBaseQueryFilter
    {
        public Guid CompanyId { get; set; }

        public string SearchFilter { get; set; } = string.Empty;

        public bool IsPrint { get; set; }

        public Guid? SaleListingId { get; set; }
    }
}
