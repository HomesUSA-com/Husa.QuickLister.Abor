namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class CommunityQueryFilter : BaseQueryFilter
    {
        public string SearchBy { get; set; }
        public string Name { get; set; }
        public XmlStatus? XmlStatus { get; set; }
    }
}
