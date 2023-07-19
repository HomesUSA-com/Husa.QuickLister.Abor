namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunitySalesOfficeResponse
    {
        public bool IsSalesOffice { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string StreetSuffix { get; set; }

        public Cities? SalesOfficeCity { get; set; }

        public string SalesOfficeZip { get; set; }
    }
}
