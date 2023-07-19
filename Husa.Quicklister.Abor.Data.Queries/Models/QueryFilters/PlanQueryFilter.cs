namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class PlanQueryFilter : BaseQueryFilter
    {
        public string SearchBy { get; set; }
        public XmlStatus? XmlStatus { get; set; }
    }
}
