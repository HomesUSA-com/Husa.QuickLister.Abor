namespace Husa.Quicklister.Abor.Data.Queries.Models.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class PlanDetailQueryResult : BaseQueryResult
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomQueryResult> Rooms { get; set; }
        public XmlStatus XmlStatus { get; set; }

        public Stories? StoriesTotal { get; set; }
        public int? SqFtTotal { get; set; }
        public int? DiningAreasTotal { get; set; }
        public int? MainLevelBedroomTotal { get; set; }
        public int? OtherLevelsBedroomTotal { get; set; }
        public int? HalfBathsTotal { get; set; }
        public int? FullBathsTotal { get; set; }
        public int? LivingAreasTotal { get; set; }
    }
}
