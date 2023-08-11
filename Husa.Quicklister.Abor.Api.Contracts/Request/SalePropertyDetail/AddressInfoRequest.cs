namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class AddressInfoRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string StreetNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string StreetName { get; set; }

        [Required]
        public Cities City { get; set; }

        [Required]
        public States State { get; set; }

        [Required]
        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "The {0} value must be {1} of length")]
        public string ZipCode { get; set; }

        public Counties? County { get; set; }
        public StreetType? StreetType { get; set; }
        public string UnitNumber { get; set; }
        public string Subdivision { get; set; }
    }
}
