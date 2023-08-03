namespace Husa.Quicklister.Abor.Api.Contracts.Response.Reports
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ScrapedListingQueryResponse
    {
        public Guid Id { get; set; }
        public MarketCode Market => MarketCode.Austin;
        public string OfficeName { get; set; }
        public string BuilderName { get; set; }
        public int? DOM { get; set; }
        public string UncleanBuilder { get; set; }
        public string MlsNum { get; set; }
        public MarketStatuses ListStatus { get; set; }
        public string Community { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal? ListPrice { get; set; }
        public int Price { get; set; }
        public string Comment { get; set; }
        public DateTime? ListDate { get; set; }
        public DateTime? Refreshed { get; set; }
        public int? Variance { get; set; }
    }
}
