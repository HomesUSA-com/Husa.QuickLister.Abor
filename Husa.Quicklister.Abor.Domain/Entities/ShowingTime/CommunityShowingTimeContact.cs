namespace Husa.Quicklister.Abor.Domain.Entities.ShowingTime
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Domain.Interfaces.ShowingTime;

    public class CommunityShowingTimeContact : IContactOrder
    {
        public Guid ScopeId { get; set; }
        public Guid ContactId { get; set; }
        public int Order { get; set; }

        public virtual CommunitySale Scope { get; set; }
        public virtual ShowingTimeContact Contact { get; set; }
    }
}
