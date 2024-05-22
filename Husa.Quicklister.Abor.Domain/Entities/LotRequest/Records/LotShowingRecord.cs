namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public record LotShowingRecord : IProvideLotShowing
    {
        [Required]
        [MinLength(1)]
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }

        [Required]
        public ICollection<ShowingContactType> ShowingContactType { get; set; }
        public string ApptPhone { get; set; }
        public string ShowingServicePhone { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PublicRemarks { get; set; }

        public string OwnerName { get; set; }

        public string ShowingInstructions { get; set; }

        [MaxLength(2000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }
        public string ShowingContactName { get; set; }

        public LotShowingRecord CloneRecord() => (LotShowingRecord)this.MemberwiseClone();
        public static LotShowingRecord CreateRecord(LotShowingInfo showingInfo)
        {
            if (showingInfo == null)
            {
                return new();
            }

            return new()
            {
                ShowingRequirements = showingInfo.ShowingRequirements,
                ApptPhone = showingInfo.ApptPhone,
                Directions = showingInfo.Directions,
                OwnerName = showingInfo.OwnerName,
                PublicRemarks = showingInfo.PublicRemarks,
                ShowingContactType = showingInfo.ShowingContactType,
                ShowingInstructions = showingInfo.ShowingInstructions,
                ShowingServicePhone = showingInfo.ShowingServicePhone,
                ShowingContactName = showingInfo.ShowingContactName,
            };
        }
    }
}
