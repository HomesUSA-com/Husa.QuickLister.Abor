namespace Husa.Quicklister.Abor.Application.Models.Request
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingRequestDto
    {
        public virtual Guid Id { get; set; }

        public virtual decimal ListPrice { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual DateTime? ListDate { get; set; }

        public virtual ListType ListType { get; set; }

        public virtual string MlsNumber { get; set; }

        public virtual MarketStatuses MlsStatus { get; set; }
    }
}
