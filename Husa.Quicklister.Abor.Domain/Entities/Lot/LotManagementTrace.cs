namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;

    public class LotManagementTrace : Entity
    {
        public LotManagementTrace(LotListing listing, Guid companyId, bool manuallyManaged)
        : this()
        {
            this.ListingId = listing.Id;
            this.CompanyId = companyId;
            this.IsManuallyManaged = manuallyManaged;
        }

        protected LotManagementTrace()
        : base()
        {
        }

        public Guid ListingId { get; set; }

        public bool IsManuallyManaged { get; set; }

        public virtual LotListing Listing { get; set; }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.ListingId;
            yield return this.IsManuallyManaged;
        }
    }
}
