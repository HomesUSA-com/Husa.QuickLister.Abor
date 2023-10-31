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
        public const int MaxDirectionsLength = 2000;

        private string occupantPhone;
        private string contactPhone;

        public CommunityShowingInfo(string ownerName)
            : this()
        {
            this.OwnerName = ownerName;
        }

        public CommunityShowingInfo()
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

        public virtual string ShowingInstructions { get; set; }
        public virtual ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        public virtual string Directions { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual LockBoxType? LockBoxType { get; set; }
        public ICollection<string> RealtorContactEmail { get; set; }

        public static CommunityShowingInfo ImportFromXml(SubdivisionResponse subdivision, CommunityShowingInfo showingInfo)
        {
            var importedShowingInfo = new CommunityShowingInfo();
            if (showingInfo != null)
            {
                importedShowingInfo = showingInfo.Clone();
            }

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
            clonedShowing.OccupantPhone = info.OccupantPhone;
            clonedShowing.ContactPhone = info.ContactPhone;
            clonedShowing.ShowingInstructions = info.ShowingInstructions;
            clonedShowing.Directions = info.Directions;
            clonedShowing.ShowingRequirements = info.ShowingRequirements;
            clonedShowing.RealtorContactEmail = info.RealtorContactEmail;
            clonedShowing.LockBoxType = info.LockBoxType;
            clonedShowing.OwnerName = info.OwnerName;

            return clonedShowing;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.OccupantPhone;
            yield return this.ContactPhone;
            yield return this.ShowingInstructions;
            yield return this.ShowingRequirements;
            yield return this.Directions;
            yield return this.LockBoxType;
            yield return this.OwnerName;
            yield return this.RealtorContactEmail;
        }
    }
}
