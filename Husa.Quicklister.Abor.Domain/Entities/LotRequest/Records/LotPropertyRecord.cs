namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public record LotPropertyRecord : IProvideLotProperty
    {
        public MlsArea? MlsArea { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<LotDescription> LotDescription { get; set; }

        [Required]
        public PropertySubType? PropertyType { get; set; }

        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }

        public LotPropertyRecord CloneRecord() => (LotPropertyRecord)this.MemberwiseClone();

        public static LotPropertyRecord CreateRecord(LotPropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return new();
            }

            return new()
            {
                MlsArea = propertyInfo.MlsArea,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
            };
        }
    }
}
