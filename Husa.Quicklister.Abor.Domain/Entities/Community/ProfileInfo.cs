namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class ProfileInfo : ValueObject, IProvideGeocodes
    {
        private string officePhone;
        private string backupPhone;

        public ProfileInfo(string name, string ownerName)
        {
            this.Name = name;
            this.OwnerName = ownerName;
        }

        public ProfileInfo()
        {
        }

        public string Name { get; set; }

        public string OwnerName { get; set; }

        public string OfficePhone
        {
            get { return this.officePhone.CleanPhoneValue(); }
            set { this.officePhone = value.CleanPhoneValue(); }
        }

        public string BackupPhone
        {
            get { return this.backupPhone.CleanPhoneValue(); }
            set { this.backupPhone = value.CleanPhoneValue(); }
        }

        public string Fax { get; set; }

        public bool UseLatLong { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public ICollection<string> EmailMailViolationsWarnings { get; set; }

        public static ProfileInfo ImportFromXml(SubdivisionResponse subdivision, string companyName, ProfileInfo profile)
        {
            var importedProfile = new ProfileInfo();
            if (profile != null)
            {
                importedProfile = profile.Clone();
            }

            importedProfile.Name = subdivision.Name;
            importedProfile.Latitude = subdivision.Latitude;
            importedProfile.Longitude = subdivision.Longitude;
            importedProfile.UseLatLong = subdivision.Latitude.HasValue && subdivision.Longitude.HasValue;
            importedProfile.OfficePhone = subdivision.SaleOffice.Phone;
            importedProfile.Fax = subdivision.SaleOffice.Fax;
            importedProfile.OwnerName = companyName;

            return importedProfile;
        }

        public ProfileInfo UpdateFromXml(SalesOfficeResponse salesOffice, string builderName)
        {
            var clonnedFeatures = this.Clone();

            if (!string.IsNullOrEmpty(salesOffice.Phone))
            {
                clonnedFeatures.OfficePhone = salesOffice.Phone;
            }

            if (!string.IsNullOrEmpty(salesOffice.Fax))
            {
                clonnedFeatures.Fax = salesOffice.Fax;
            }

            clonnedFeatures.OwnerName = builderName;

            return clonnedFeatures;
        }

        public ProfileInfo Clone()
        {
            return (ProfileInfo)this.MemberwiseClone();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Name;
            yield return this.OwnerName;
            yield return this.OfficePhone;
            yield return this.BackupPhone;
            yield return this.Fax;
            yield return this.UseLatLong;
            yield return this.Latitude;
            yield return this.Longitude;
            yield return this.EmailMailViolationsWarnings;
        }
    }
}
