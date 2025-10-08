namespace Husa.Quicklister.Abor.Domain.Entities.ShowingTime
{
    using System;
    using Husa.Extensions.ShowingTime.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;

    public class ListingShowingTimeContact : IContactOrder
    {
        public Guid ScopeId { get; set; }
        public Guid ContactId { get; set; }
        public int Order { get; set; }

        public virtual SaleListing Scope { get; set; }
        public virtual ShowingTimeContact Contact { get; set; }
    }
}
