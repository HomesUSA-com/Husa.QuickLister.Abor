namespace Husa.Quicklister.Abor.Api.Contracts.Response.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class PlanDetailResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomResponse> Rooms { get; set; }
        public DateTime? SysModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public Guid? SysCreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Guid? SysModifiedBy { get; set; }
        public DateTime SysCreatedOn { get; set; }
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
