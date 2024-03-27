namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record AddressRecord : IProvideSummary
    {
        private string subdivision;

        public const string SummarySection = "AddressInfo";
        public string FormalAddress { get; set; }
        public string ReadableCity { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }

        [Required]
        public Cities City { get; set; }

        [Required]
        public States State { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "The {0} value must be {1} of length")]
        public string ZipCode { get; set; }

        [Required]
        public Counties County { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

        [Required]
        public StreetType? StreetType { get; set; }
        public string UnitNumber { get; set; }

        public AddressRecord CloneRecord() => (AddressRecord)this.MemberwiseClone();

        public static AddressRecord CreateRecord(AddressInfo addressInfo)
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
                County = addressInfo.County ?? throw new DomainException(nameof(addressInfo.County)),
                Subdivision = addressInfo.Subdivision,
                StreetType = addressInfo.StreetType ?? throw new DomainException(nameof(addressInfo.StreetType)),
                UnitNumber = addressInfo.UnitNumber,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
