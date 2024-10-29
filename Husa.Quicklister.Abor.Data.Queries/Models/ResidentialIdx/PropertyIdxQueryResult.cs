namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PropertyIdxQueryResult
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
