namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
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

        public string EmailMailViolationsWarnings { get; set; }

        public CommunitySalesOfficeResponse SalesOffice { get; set; }

        public EmailLeadResponse EmailLead { get; set; }
    }
}
