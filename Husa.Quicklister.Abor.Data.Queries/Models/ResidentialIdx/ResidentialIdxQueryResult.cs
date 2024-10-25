namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx.Media;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ResidentialIdxQueryResult
    {
        public Guid Id { get; set; }
        public Guid? XmlId { get; set; }
        public decimal? ListPrice { get; set; }
        public DateTime? ListDate { get; set; }
        public string MlsNumber { get; set; }
        public string SpecNumber { get; set; }
        public MarketCode MarketCode { get; set; }
        public MarketStatuses MlsStatus { get; set; }
        public AddressQueryResult Address { get; set; }
        public MediaIdxQueryResult Media { get; set; }
    }
}
