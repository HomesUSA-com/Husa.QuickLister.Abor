namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

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
        [MaxLength(5, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string LotNum { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Block { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

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
                LotNum = addressInfo.LotNum,
                Block = addressInfo.Block,
                Subdivision = addressInfo.Subdivision,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
