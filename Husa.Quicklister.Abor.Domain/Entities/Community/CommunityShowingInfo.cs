namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class CommunityShowingInfo : ValueObject, IProvideShowingInfo
    {
        public const int MaxDirectionsLength = 255;

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

        public virtual Showing? Showing { get; set; }
        public virtual string RealtorContactEmail { get; set; }
        public virtual string Directions { get; set; }

        public static CommunityShowingInfo ImportFromXml(SubdivisionResponse subdivision, CommunityShowingInfo showingInfo)
        {
            var importedShowingInfo = new CommunityShowingInfo();
            if (showingInfo != null)
            {
                importedShowingInfo = showingInfo.Clone();
            }

            importedShowingInfo.RealtorContactEmail = subdivision.SaleOffice.Email;
            if (string.IsNullOrEmpty(subdivision.DrivingDirections))
            {
                return importedShowingInfo;
            }

            var directions = subdivision.DrivingDirections;
            if (subdivision.DrivingDirections.Length > MaxDirectionsLength)
            {
                directions = subdivision.DrivingDirections[..MaxDirectionsLength];
            }

            importedShowingInfo.Directions = directions;
            return importedShowingInfo;
        }

        public virtual CommunityShowingInfo UpdateFromXml(SubdivisionResponse subdivision)
        {
            var clonnedShowing = this.Clone();

            if (!string.IsNullOrEmpty(subdivision.SaleOffice.Email))
            {
                clonnedShowing.RealtorContactEmail = subdivision.SaleOffice.Email;
            }

            if (!string.IsNullOrEmpty(subdivision.DrivingDirections))
            {
                var directions = subdivision.DrivingDirections;
                if (subdivision.DrivingDirections.Length > MaxDirectionsLength)
                {
                    directions = subdivision.DrivingDirections[..MaxDirectionsLength];
                }

                clonnedShowing.Directions = directions;
            }

            return clonnedShowing;
        }

        public CommunityShowingInfo Clone()
        {
            return (CommunityShowingInfo)this.MemberwiseClone();
        }

        public virtual CommunityShowingInfo ImportShowing(ShowingInfo info)
        {
            var clonedShowing = this.Clone();
            clonedShowing.AltPhoneCommunity = info.AltPhoneCommunity;
            clonedShowing.AgentListApptPhone = info.AgentListApptPhone;
            clonedShowing.RealtorContactEmail = info.RealtorContactEmail;
            clonedShowing.Showing = info.Showing;
            clonedShowing.Directions = info.Directions;

            return clonedShowing;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.AltPhoneCommunity;
            yield return this.AgentListApptPhone;
            yield return this.Showing;
            yield return this.RealtorContactEmail;
            yield return this.Directions;
        }
    }
}
