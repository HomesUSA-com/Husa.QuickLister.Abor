namespace Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx.Media;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ResidentialIdxResponse
    {
        public decimal? ListPrice { get; set; }
        public DateTime? ListDate { get; set; }
        public string MlsNumber { get; set; }
        public string SpecNumber { get; set; }
        public MarketCode MarketCode { get; set; }
        public MarketStatuses MlsStatus { get; set; }
        public AddressInfoResponse Address { get; set; }
        public MediaIdxResponse Media { get; set; }
    }
}
