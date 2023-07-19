namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ReversePorspectListingQueryResult
    {
        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public Guid ListingID { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public Cities City { get; set; }

        public States State { get; set; }

        public string ZipCode { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public Guid CompanyId { get; set; }
    }
}
