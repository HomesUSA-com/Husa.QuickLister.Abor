namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunitySalesOfficeDto
    {
        public bool IsSalesOffice { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string StreetSuffix { get; set; }

        public Cities? SalesOfficeCity { get; set; }

        public string SalesOfficeZip { get; set; }
    }
}
