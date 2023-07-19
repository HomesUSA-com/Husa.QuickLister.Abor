namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;

    public class ManagementTrace : Entity
    {
        public ManagementTrace(SaleListing saleListing, Guid companyId, bool manuallyManaged)
        : this()
        {
            this.SaleListingId = saleListing.Id;
            this.CompanyId = companyId;
            this.IsManuallyManaged = manuallyManaged;
        }

        protected ManagementTrace()
        : base()
        {
        }

        public Guid SaleListingId { get; set; }

        public bool IsManuallyManaged { get; set; }

        public virtual SaleListing SaleListing { get; set; }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.SaleListingId;
            yield return this.IsManuallyManaged;
        }
    }
}
