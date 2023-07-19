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
        public Stories? Stories { get; set; }
        public int? BathsFull { get; set; }
        public int? BathsHalf { get; set; }
        public int? NumBedrooms { get; set; }
        public IEnumerable<GarageDescription> GarageDescription { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomDto> Rooms { get; set; }
    }
}
