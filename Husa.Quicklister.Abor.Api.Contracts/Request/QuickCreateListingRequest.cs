namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.ValidationAttributes;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class QuickCreateListingRequest
    {
        private string streetName;

        [EnumDataType(typeof(MarketStatuses), ErrorMessage = "{0} value is not valid.")]
        public MarketStatuses MlsStatus { get; set; }

        public string StreetName
        {
            get { return this.streetName; }
            set { this.streetName = value.Trim(); }
        }

        public string StreetNumber { get; set; }

        public decimal? ListPrice { get; set; }

        public Cities City { get; set; }

        public States? State { get; set; }

        public string UnitNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(5, ErrorMessage = "The {0} value must be 5 characters.")]
        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public DateTime? ConstructionCompletionDate { get; set; }

        public Guid CompanyId { get; set; }

        [RequiredIfNull(nameof(ListingIdToImport))]
        public Guid? CommunityId { get; set; }

        public Guid? PlanId { get; set; }

        [RequiredIfNull(nameof(CommunityId))]
        public Guid? ListingIdToImport { get; set; }

        public bool IsManuallyManaged { get; set; }
    }
}
