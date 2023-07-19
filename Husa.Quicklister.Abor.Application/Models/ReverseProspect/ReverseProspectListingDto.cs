namespace Husa.Quicklister.Abor.Application.Models.ReverseProspect
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ReverseProspectListingDto
    {
        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public Guid ListingId { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public States State { get; set; }

        public string ZipCode { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public Guid CompanyId { get; set; }
    }
}
