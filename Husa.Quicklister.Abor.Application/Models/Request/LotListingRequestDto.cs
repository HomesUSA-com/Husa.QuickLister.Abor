namespace Husa.Quicklister.Abor.Application.Models.Request
{
    using System;
    using Husa.Quicklister.Abor.Application.Models.Lot;

    public class LotListingRequestDto : ListingRequestDto
    {
        public Guid ListingId { get; set; }
        public string OwnerName { get; set; }
        public ListingStatusFieldsDto StatusFieldsInfo { get; set; }
        public LotAddressDto AddressInfo { get; set; }
        public LotPropertyDto PropertyInfo { get; set; }
        public LotFeaturesDto FeaturesInfo { get; set; }
        public LotFinancialDto FinancialInfo { get; set; }
        public LotShowingDto ShowingInfo { get; set; }
        public LotSchoolsDto SchoolsInfo { get; set; }
    }
}
