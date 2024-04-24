namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotPropertyDto
    {
        public MlsArea? MlsArea { get; set; }
        public PropertySubType? PropertyType { get; set; }
        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        public ICollection<LotDescription> LotDescription { get; set; }
    }
}
