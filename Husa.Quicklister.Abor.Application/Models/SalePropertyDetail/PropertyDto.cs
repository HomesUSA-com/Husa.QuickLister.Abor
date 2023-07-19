namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PropertyDto
    {
        public DateTime? ConstructionCompletionDate { get; set; }

        public ConstructionStage? ConstructionStage { get; set; }

        public int? ConstructionStartYear { get; set; }

        public string LegalDescription { get; set; }

        public string TaxId { get; set; }

        public MlsArea? MlsArea { get; set; }

        public string MapscoGrid { get; set; }

        public string LotDimension { get; set; }

        public string LotSize { get; set; }

        public ICollection<LotDescription> LotDescription { get; set; }

        public ICollection<Occupancy> Occupancy { get; set; }

        public bool UpdateGeocodes { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsXmlManaged { get; set; }
    }
}
