namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using System.Collections.Generic;

    public class ProfileQueryResult
    {
        public string Name { get; set; }

        public string OwnerName { get; set; }

        public string OfficePhone { get; set; }

        public string BackupPhone { get; set; }

        public string Fax { get; set; }

        public bool UseLatLong { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public ICollection<string> EmailMailViolationsWarnings { get; set; }

        public SalesOfficeQueryResult SalesOffice { get; set; }

        public EmailLeadQueryResult EmailLead { get; set; }
    }
}
