namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Extensions.ShowingTime.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;

    public class CommunityValueObject : ValueObject
    {
        public virtual Property PropertyInfo { get; set; }

        public virtual ProfileInfo ProfileInfo { get; set; }

        public virtual CommunitySaleOffice SalesOfficeInfo { get; set; }

        public virtual EmailLead EmailLeadInfo { get; set; }

        public virtual Utilities UtilitiesInfo { get; set; }

        public virtual CommunityFinancialInfo FinancialInfo { get; set; }

        public virtual SchoolsInfo SchoolsInfo { get; set; }

        public virtual CommunityShowingInfo ShowingInfo { get; set; }

        public virtual ShowingTime ShowingTime { get; set; }
        public virtual bool UseShowingTime { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.PropertyInfo;
            yield return this.ProfileInfo;
            yield return this.SalesOfficeInfo;
            yield return this.EmailLeadInfo;
            yield return this.UtilitiesInfo;
            yield return this.FinancialInfo;
            yield return this.SchoolsInfo;
            yield return this.ShowingInfo;
            yield return this.ShowingTime;
            yield return this.UseShowingTime;
        }
    }
}
