namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingDto
    {
        public Guid Id { get; set; }

        public decimal? ListPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ListDate { get; set; }

        public ListType ListType { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public bool IsManuallyManaged { get; set; }
    }
}
