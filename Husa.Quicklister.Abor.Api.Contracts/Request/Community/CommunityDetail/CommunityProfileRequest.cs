namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Community;

    public class CommunityProfileRequest
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

        public CommunitySalesOfficeRequest SalesOffice { get; set; }

        public EmailLeadRequest EmailLead { get; set; }
    }
}
