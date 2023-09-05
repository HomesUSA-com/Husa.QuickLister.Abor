namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest.SaleRequest;

    public class ListingSaleRequestQueryResponse : ISaleListingRequestResponse
    {
        public Guid Id { get; set; }

        public string OwnerName { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Market { get; set; }

        public Cities City { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public string Address { get; set; }

        public decimal? ListPrice { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public string CreatedBy { get; set; }

        public bool EnableOpenHouse { get; set; }
    }
}
