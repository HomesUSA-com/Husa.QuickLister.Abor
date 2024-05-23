namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public record LotAddressRecord : IProvideLotAddress
    {
        private string subdivision;

        public const string SummarySection = "AddressInfo";
        public string FormalAddress { get; set; }
        public string ReadableCity { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }

        [Required]
        public Cities City { get; set; }

        [Required]
        public States State { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "The {0} value must be {1} of length")]
        public string ZipCode { get; set; }

        [Required]
        public Counties? County { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

        [Required]
        public StreetType? StreetType { get; set; }
        public string UnitNumber { get; set; }

        public LotAddressRecord CloneRecord() => (LotAddressRecord)this.MemberwiseClone();

        public static LotAddressRecord CreateRecord(LotAddressInfo addressInfo)
        {
            if (addressInfo == null)
            {
                return new();
            }

            return new()
            {
                FormalAddress = addressInfo.FormalAddress,
                ReadableCity = addressInfo.ReadableCity,
                StreetNumber = addressInfo.StreetNumber,
                StreetName = addressInfo.StreetName,
                City = addressInfo.City,
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                County = addressInfo.County,
                Subdivision = addressInfo.Subdivision,
                StreetType = addressInfo.StreetType,
                StreetDirPrefix = addressInfo.StreetDirPrefix,
                StreetDirSuffix = addressInfo.StreetDirSuffix,
            };
        }
    }
}
