namespace Husa.Quicklister.Abor.Application.Models.Listing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingDto
    {
        public int? CDOM { get; set; }

        public int? DOM { get; set; }

        public decimal ListPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ListDate { get; set; }

        public ListType ListType { get; set; }

        public DateTime MarketModifiedOn { get; set; }

        public string MarketUniqueId { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public Guid PropertyId { get; set; }
    }
}
