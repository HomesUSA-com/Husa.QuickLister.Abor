namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingRequest
    {
        public Guid Id { get; set; }

        public decimal? ListPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ListDate { get; set; }

        public virtual ListType ListType { get; set; }

        public string MlsNumber { get; set; }

        [EnumDataType(typeof(MarketStatuses), ErrorMessage = "{0} value is not valid.")]
        public MarketStatuses MlsStatus { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public bool IsManuallyManaged { get; set; }
    }
}
