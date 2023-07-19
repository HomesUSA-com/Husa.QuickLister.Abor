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
        public Stories? Stories { get; set; }
        public int? BathsFull { get; set; }
        public int? BathsHalf { get; set; }
        public int? NumBedrooms { get; set; }
        public IEnumerable<GarageDescription> GarageDescription { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomQueryResult> Rooms { get; set; }
        public XmlStatus XmlStatus { get; set; }
    }
}
