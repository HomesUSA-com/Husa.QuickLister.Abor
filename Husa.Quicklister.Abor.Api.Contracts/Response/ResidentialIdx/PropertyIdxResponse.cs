namespace Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PropertyIdxResponse
    {
        public DateTime? ConstructionCompletionDate { get; set; }
        public ConstructionStage? ConstructionStage { get; set; }
        public int? ConstructionStartYear { get; set; }
        public string LotSize { get; set; }
        public PropertySubType? PropertyType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
