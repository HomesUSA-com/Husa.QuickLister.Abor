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
        private string occupantPhone;
        private string contactPhone;

        public ShowingInfo(string ownerName)
            : this()
        {
            this.OwnerName = ownerName;
        }

        public ShowingInfo()
        {
        }

        public virtual string OccupantPhone
        {
            get { return this.occupantPhone; }
            set { this.occupantPhone = value.CleanPhoneValue(); }
        }

        public virtual string ContactPhone
        {
            get { return this.contactPhone; }
            set { this.contactPhone = value.CleanPhoneValue(); }
        }

        public virtual string AgentPrivateRemarks { get; set; }
        public virtual string AgentPrivateRemarksAdditional { get; set; }
        public virtual string LockBoxSerialNumber { get; set; }
        public virtual string ShowingInstructions { get; set; }
        public virtual ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        public virtual ICollection<LockBoxType> LockBoxType { get; set; }
        public virtual string RealtorContactEmail { get; set; }
        public virtual string Directions { get; set; }
        public virtual string OwnerName { get; set; }
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
            clonnedShowing.OccupantPhone = showing.OccupantPhone;
            clonnedShowing.ContactPhone = showing.ContactPhone;
            clonnedShowing.ShowingInstructions = showing.ShowingInstructions;
            clonnedShowing.ShowingRequirements = showing.ShowingRequirements;
            clonnedShowing.LockBoxType = showing.LockBoxType;
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
            yield return this.OccupantPhone;
            yield return this.ContactPhone;
            yield return this.AgentPrivateRemarks;
            yield return this.AgentPrivateRemarksAdditional;
            yield return this.LockBoxSerialNumber;
            yield return this.ShowingInstructions;
            yield return this.ShowingRequirements;
            yield return this.LockBoxType;
            yield return this.RealtorContactEmail;
            yield return this.Directions;
            yield return this.EnableOpenHouses;
            yield return this.OpenHousesAgree;
            yield return this.ShowOpenHousesPending;
            yield return this.OwnerName;
        }
    }
}
