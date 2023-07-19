namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ShowingInfo : ValueObject, IProvideShowingInfo
    {
        private string altPhoneCommunity;
        private string agentListApptPhone;

        public virtual string AltPhoneCommunity
        {
            get { return this.altPhoneCommunity.CleanPhoneValue(); }
            set { this.altPhoneCommunity = value.CleanPhoneValue(); }
        }

        public virtual string AgentListApptPhone
        {
            get { return this.agentListApptPhone.CleanPhoneValue(); }
            set { this.agentListApptPhone = value.CleanPhoneValue(); }
        }

        public virtual string AgentPrivateRemarks { get; set; }

        public virtual Showing? Showing { get; set; }

        public virtual string RealtorContactEmail { get; set; }

        public virtual string Directions { get; set; }

        public virtual bool EnableOpenHouses { get; protected set; }

        public virtual bool OpenHousesAgree { get; protected set; }

        public virtual bool ShowOpenHousesPending { get; protected set; }

        public ShowingInfo Clone()
        {
            return (ShowingInfo)this.MemberwiseClone();
        }

        public ShowingInfo ImportShowingFromCommunity(CommunityShowingInfo showing)
        {
            var clonnedShowing = this.Clone();
            clonnedShowing.AltPhoneCommunity = showing.AltPhoneCommunity;
            clonnedShowing.AgentListApptPhone = showing.AgentListApptPhone;
            clonnedShowing.RealtorContactEmail = showing.RealtorContactEmail;
            clonnedShowing.Showing = showing.Showing;
            clonnedShowing.Directions = showing.Directions;

            return clonnedShowing;
        }

        public virtual void EnableOpenHouse(bool showOpenHouseWhenPending = false)
        {
            this.EnableOpenHouses = true;
            this.OpenHousesAgree = true;

            if (showOpenHouseWhenPending)
            {
                this.ShowOpenHousesPending = true;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.AltPhoneCommunity;
            yield return this.AgentListApptPhone;
            yield return this.Showing;
            yield return this.RealtorContactEmail;
            yield return this.Directions;
            yield return this.AgentPrivateRemarks;
            yield return this.EnableOpenHouses;
            yield return this.OpenHousesAgree;
            yield return this.ShowOpenHousesPending;
        }
    }
}
