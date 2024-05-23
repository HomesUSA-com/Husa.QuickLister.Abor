namespace Husa.Quicklister.Abor.Domain.Extensions.Lot
{
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public static class ImportDataExtensions
    {
        public static void ImportDataFromListing(this LotListing listing, LotListing saleListingToClone)
        {
            listing.CommunityId = saleListingToClone.CommunityId;
            listing.CompanyId = saleListingToClone.CompanyId;
            listing.OwnerName = saleListingToClone.OwnerName;
            listing.AddressInfo.PartialClone(saleListingToClone.AddressInfo);
            listing.PropertyInfo.PartialClone(saleListingToClone.PropertyInfo);

            listing.FeaturesInfo = saleListingToClone.FeaturesInfo.Clone();
            listing.FinancialInfo = saleListingToClone.FinancialInfo.Clone();
            listing.SchoolsInfo = saleListingToClone.SchoolsInfo.Clone();
            listing.ShowingInfo = saleListingToClone.ShowingInfo.Clone();
        }

        public static void ImportDataFromCommunity(this LotListing listing, CommunitySale communitySale)
        {
            listing.CommunityId = communitySale.Id;
            listing.SchoolsInfo = listing.SchoolsInfo.ImportSchools(communitySale.SchoolsInfo);
            listing.FeaturesInfo = listing.FeaturesInfo.ImportFeaturesFromCommunity(communitySale.Utilities);
            listing.FinancialInfo = listing.FinancialInfo.ImportFinancialFromCommunity(communitySale.Financial);
            listing.ShowingInfo = listing.ShowingInfo.ImportShowingFromCommunity(communitySale.Showing);
            listing.AddressInfo = listing.AddressInfo.ImportAddressInfoFromCommunity(communitySale.Property);
            listing.PropertyInfo = listing.PropertyInfo.ImportPropertyFromCommunity(communitySale.Property);
        }
    }
}
