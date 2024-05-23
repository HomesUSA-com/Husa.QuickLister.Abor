namespace Husa.Quicklister.Abor.Domain.Extensions.Lot
{
    using System;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public static class UpdatePropertiesExtensions
    {
        public static void UpdateFeatures(this LotListing listing, LotFeaturesInfo features)
        {
            ArgumentNullException.ThrowIfNull(features);
            if (listing.FeaturesInfo != features)
            {
                listing.FeaturesInfo = features;
            }
        }

        public static void UpdateFinancial(this LotListing listing, LotFinancialInfo financial)
        {
            ArgumentNullException.ThrowIfNull(financial);

            if (!financial.IsValidBuyersAgentCommissionRange())
            {
                throw new DomainException($"The range for Buyers Agent Commission is invalid for type {financial.BuyersAgentCommissionType}");
            }

            if (financial.HasAgentBonus && !financial.IsValidAgentBonusAmountRange())
            {
                throw new DomainException($"The range for Agent bonus amount is invalid for type {financial.BuyersAgentCommissionType}");
            }

            if (listing.FinancialInfo != financial)
            {
                listing.FinancialInfo = financial;
            }
        }

        public static void UpdatePropertyInfo(this LotListing listing, LotPropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo);
            if (listing.PropertyInfo != propertyInfo)
            {
                listing.PropertyInfo = propertyInfo;
            }
        }

        public static void UpdateAddressInfo(this LotListing listing, LotAddressInfo addressInfo)
        {
            ArgumentNullException.ThrowIfNull(addressInfo);

            if (listing.AddressInfo != addressInfo)
            {
                listing.AddressInfo = addressInfo;
            }
        }

        public static void UpdateShowing(this LotListing listing, LotShowingInfo showing)
        {
            ArgumentNullException.ThrowIfNull(showing);
            if (listing.ShowingInfo != showing)
            {
                listing.ShowingInfo = showing;
            }
        }

        public static void UpdateSchools(this LotListing listing, LotSchoolsInfo schools)
        {
            ArgumentNullException.ThrowIfNull(schools);
            if (listing.SchoolsInfo != schools)
            {
                listing.SchoolsInfo = schools;
            }
        }
    }
}
