namespace Husa.Quicklister.Abor.Application.Models.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class UpdatePlanDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomDto> Rooms { get; set; }
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
