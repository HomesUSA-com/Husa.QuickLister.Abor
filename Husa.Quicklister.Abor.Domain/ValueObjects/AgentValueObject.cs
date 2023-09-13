  namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Domain.ValueObjects;

    public class AgentValueObject : ValueObject
    {
        public string MarketUniqueId { get; set; }

        public string OfficeId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public MemberStatus Status { get; set; }

        public string CellPhone { get; set; }

        public string WorkPhone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string HomePhone { get; set; }

        public string Web { get; set; }

        public DateTimeOffset MarketModified { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MarketUniqueId;
            yield return this.OfficeId;
            yield return this.FirstName;
            yield return this.MiddleName;
            yield return this.LastName;
            yield return this.FullName;
            yield return this.Status;
            yield return this.CellPhone;
            yield return this.WorkPhone;
            yield return this.HomePhone;
            yield return this.Web;
            yield return this.Email;
            yield return this.Fax;
            yield return this.HomePhone;
            yield return this.MarketModified;
        }
    }
}
