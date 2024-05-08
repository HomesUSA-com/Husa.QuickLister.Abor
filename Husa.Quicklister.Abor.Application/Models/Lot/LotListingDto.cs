namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using System;

    public class LotListingDto : ListingDto
    {
        public string OwnerName { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CommunityId { get; set; }

        public SalePropertyDetail.AddressDto AddressInfo { get; set; }
        public LotPropertyDto PropertyInfo { get; set; }
        public LotFeaturesDto FeaturesInfo { get; set; }
        public LotFinancialDto FinancialInfo { get; set; }
        public LotShowingDto ShowingInfo { get; set; }
        public LotSchoolsDto SchoolsInfo { get; set; }
    }
}
