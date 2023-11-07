namespace Husa.Quicklister.Abor.Data.Queries.Models.Xml
{
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class XmlListingQueryFilter : BaseAlertQueryFilter
    {
        public ImportStatus ImportStatus { get; set; }
    }
}
