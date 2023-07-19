namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class CommunityQueryFilter : BaseQueryFilter
    {
        public string SearchBy { get; set; }
        public string Name { get; set; }
        public XmlStatus? XmlStatus { get; set; }
    }
}
