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
            };
        }
    }
}
