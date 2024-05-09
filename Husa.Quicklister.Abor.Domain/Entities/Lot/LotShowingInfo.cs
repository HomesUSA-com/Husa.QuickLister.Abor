namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public class LotShowingInfo : ValueObject, IProvideLotShowing
    {
        private string apptPhone;
        private string showingServicePhone;

        public virtual ICollection<ShowingRequirements> ShowingRequirements { get; set; }

        public virtual string ApptPhone
        {
            get { return this.apptPhone; }
            set { this.apptPhone = value.CleanPhoneValue(); }
        }

        public virtual string ShowingServicePhone
        {
            get { return this.showingServicePhone; }
            set { this.showingServicePhone = value.CleanPhoneValue(); }
        }

        public string OwnerName { get; set; }
        public string ShowingInstructions { get; set; }
        public string PublicRemarks { get; set; }
        public string Directions { get; set; }
        public ICollection<ShowingContactType> ShowingContactType { get; set; }

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
            yield return this.ApptPhone;
            yield return this.OwnerName;
            yield return this.ShowingContactType;
            yield return this.ShowingInstructions;
            yield return this.ShowingServicePhone;
            yield return this.PublicRemarks;
            yield return this.Directions;
        }
    }
}
