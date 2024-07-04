namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;

    public class CommunityProfileResponse
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

        public CommunitySalesOfficeResponse SalesOffice { get; set; }

        public EmailLeadResponse EmailLead { get; set; }
    }
}
