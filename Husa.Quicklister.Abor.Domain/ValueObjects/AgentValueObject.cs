  namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;

    public class AgentValueObject : ValueObject
    {
        public string MarketUniqueId { get; set; }

        public string LoginName { get; set; }

        public string OfficeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Status { get; set; }

        public string CellPhone { get; set; }

        public string WorkPhone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string OtherPhone { get; set; }

        public DateTime MarketModified { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MarketUniqueId;
            yield return this.LoginName;
            yield return this.OfficeId;
            yield return this.FirstName;
            yield return this.LastName;
            yield return this.Status;
            yield return this.CellPhone;
            yield return this.WorkPhone;
            yield return this.Email;
            yield return this.Fax;
            yield return this.OtherPhone;
            yield return this.MarketModified;
        }
    }
}
