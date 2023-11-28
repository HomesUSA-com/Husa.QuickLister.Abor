namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using System.Collections.Generic;

    public class CommunityProfileDto
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

        public CommunitySalesOfficeDto SalesOffice { get; set; }

        public EmailLeadDto EmailLead { get; set; }
    }
}
