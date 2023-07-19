namespace Husa.Quicklister.Abor.Data.Documents.QueryFilters
{
    using System;

    public class ListingSaleRequestByCompanyQueryFilter : RequestBaseQueryFilter
    {
        public Guid CompanyId { get; set; }
    }
}
