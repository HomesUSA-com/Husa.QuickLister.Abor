namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingQueryFilter : BaseQueryFilter
    {
        public string SearchBy { get; set; }
        public IEnumerable<MarketStatuses> MlsStatus { get; set; }
        public ListedType? ListedType { get; set; }
        public Guid? CommunityId { get; set; }
        public Guid? PlanId { get; set; }
        public int? SqftMin { get; set; }
        public int? SqftMax { get; set; }
        public DateTime? CreationStartDate { get; set; }
        public DateTime? CreationEndDate { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string MlsNumber { get; set; }
        public bool? IsCompleteHome { get; set; }
    }
}
