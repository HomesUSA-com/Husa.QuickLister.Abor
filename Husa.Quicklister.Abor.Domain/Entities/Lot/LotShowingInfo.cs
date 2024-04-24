namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingInfo : ValueObject
    {
        public virtual ICollection<ShowingRequirements> ShowingRequirements { get; set; }

        public LotShowingInfo Clone()
        {
            return (LotShowingInfo)this.MemberwiseClone();
        }

        public LotShowingInfo ImportShowingFromCommunity(CommunityShowingInfo showing)
        {
            var clonnedShowing = this.Clone();
            clonnedShowing.ShowingRequirements = showing.ShowingRequirements;
            return clonnedShowing;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.ShowingRequirements;
        }
    }
}
