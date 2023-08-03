namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityPropertyRequest
    {
        public Cities? City { get; set; }

        public Counties? County { get; set; }

        public string ZipCode { get; set; }

        public string Subdivision { get; set; }

        public string LotSize { get; set; }

        public MlsArea? MlsArea { get; set; }

        public ConstructionStage? ConstructionStage { get; set; }

        public string LotDimension { get; set; }

        public ICollection<LotDescription> LotDescription { get; set; }

        public PropertySubType? PropertyType { get; set; }
    }
}
