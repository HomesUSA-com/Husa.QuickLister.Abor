namespace Husa.Quicklister.Abor.Domain.Extensions.Lot
{
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;

    public static class CommunitySubmitExtensions
    {
        public static bool UpdateListingFromCommunitySubmit(this LotListing listing)
        {
            var oldProperty = listing.Clone();

            listing.ImportDataFromCommunitySubmit(listing.Community);

            var hasChanges = !listing.IsEqualTo(oldProperty);

            return hasChanges;
        }

        public static void ImportDataFromCommunitySubmit(this LotListing listing, CommunitySale communitySale)
        {
            communitySale.SchoolsInfo.CopyProperties(listing.SchoolsInfo, communitySale.GetChangedProperties(nameof(communitySale.SchoolsInfo)));
            communitySale.Financial.CopyProperties(listing.FinancialInfo, communitySale.GetChangedProperties(nameof(communitySale.Financial)));
            communitySale.Showing.CopyProperties(listing.ShowingInfo, communitySale.GetChangedProperties(nameof(communitySale.Showing)));

            var utilitiesChanges = communitySale.GetChangedProperties(nameof(communitySale.Utilities));
            communitySale.Utilities.CopyProperties(listing.FeaturesInfo, utilitiesChanges);

            var propertyChanges = communitySale.GetChangedProperties(nameof(communitySale.Property));
            communitySale.Property.CopyProperties(listing.AddressInfo, propertyChanges);
            communitySale.Property.CopyProperties(listing.PropertyInfo, propertyChanges);
        }

        public static void ImportDataFromCommunitySubmit(this LotListingRequest listing, CommunitySale communitySale)
        {
            communitySale.SchoolsInfo.CopyProperties(listing.SchoolsInfo, communitySale.GetChangedProperties(nameof(communitySale.SchoolsInfo)));
            communitySale.Financial.CopyProperties(listing.FinancialInfo, communitySale.GetChangedProperties(nameof(communitySale.Financial)));
            communitySale.Showing.CopyProperties(listing.ShowingInfo, communitySale.GetChangedProperties(nameof(communitySale.Showing)));

            var utilitiesChanges = communitySale.GetChangedProperties(nameof(communitySale.Utilities));
            communitySale.Utilities.CopyProperties(listing.FeaturesInfo, utilitiesChanges);

            var propertyChanges = communitySale.GetChangedProperties(nameof(communitySale.Property));
            communitySale.Property.CopyProperties(listing.AddressInfo, propertyChanges);
            communitySale.Property.CopyProperties(listing.PropertyInfo, propertyChanges);
        }
    }
}
