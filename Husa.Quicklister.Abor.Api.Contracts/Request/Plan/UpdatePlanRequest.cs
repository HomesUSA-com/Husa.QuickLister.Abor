namespace Husa.Quicklister.Abor.Api.Contracts.Request.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class UpdatePlanRequest
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public virtual Stories? Stories { get; set; }
        public virtual int? BathsFull { get; set; }
        public virtual int? BathsHalf { get; set; }
        public virtual int? NumBedrooms { get; set; }
        public virtual ICollection<GarageDescription> GarageDescription { get; set; }
        public bool IsNewConstruction { get; set; }
        public IEnumerable<RoomRequest> Rooms { get; set; }
    }
}
