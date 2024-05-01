namespace Husa.Quicklister.Abor.Data.Queries.Extensions.Lot
{
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public static class LotQueryExtensions
    {
        public static SchoolsQueryResult ToProjectionSchools<T>(this T schools)
            where T : class, IProvideSchool
        {
            if (schools == null)
            {
                return new();
            }

            return new()
            {
                SchoolDistrict = schools.SchoolDistrict,
                ElementarySchool = schools.ElementarySchool,
                MiddleSchool = schools.MiddleSchool,
                HighSchool = schools.HighSchool,
            };
        }

        public static LotPropertyQueryResult ToProjectionPropertyInfo<T>(this T propertyInfo)
            where T : class, IProvideLotProperty
        {
            if (propertyInfo == null)
            {
                return new();
            }

            return new()
            {
                MlsArea = propertyInfo.MlsArea,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
            };
        }

        public static LotFinancialQueryResult ToProjectionFinancial<T>(this T financial)
            where T : class, IProvideLotFinancial
        {
            if (financial == null)
            {
                return new();
            }

            return new()
            {
                TaxRate = financial.TaxRate,
                AcceptableFinancing = financial.AcceptableFinancing,
                HoaIncludes = financial.HoaIncludes,
                HasHoa = financial.HasHoa,
                BillingFrequency = financial.BillingFrequency,
                HOARequirement = financial.HOARequirement,
                BuyersAgentCommission = financial.BuyersAgentCommission,
                BuyersAgentCommissionType = financial.BuyersAgentCommissionType,
                HasAgentBonus = financial.HasAgentBonus,
                HasBonusWithAmount = financial.HasBonusWithAmount,
                AgentBonusAmount = financial.AgentBonusAmount,
                AgentBonusAmountType = financial.AgentBonusAmountType,
                BonusExpirationDate = financial.BonusExpirationDate,
                HasBuyerIncentive = financial.HasBuyerIncentive,
            };
        }

        public static LotFeaturesQueryResult ToProjectionFeatures<T>(this T features)
            where T : class, IProvideLotFeatures
        {
            if (features == null)
            {
                return new();
            }

            return new()
            {
                RestrictionsDescription = features.RestrictionsDescription,
                UtilitiesDescription = features.UtilitiesDescription,
                WaterSource = features.WaterSource,
                WaterSewer = features.WaterSewer,
                Fencing = features.Fencing,
                View = features.View,
                ExteriorFeatures = features.ExteriorFeatures,
                WaterfrontFeatures = features.WaterfrontFeatures,
                DistanceToWaterAccess = features.DistanceToWaterAccess,
            };
        }

        public static LotShowingQueryResult ToProjectionShowing<T>(this T showing)
            where T : class, IProvideLotShowing
        {
            if (showing == null)
            {
                return new();
            }

            return new()
            {
                ShowingRequirements = showing.ShowingRequirements,
            };
        }

        public static AddressQueryResult ToProjectionAddressInfo<TStatusFields>(this TStatusFields addressInfo)
           where TStatusFields : class, IProvideLotAddress
        {
            if (addressInfo == null)
            {
                return new();
            }

            return new()
            {
                StreetNumber = addressInfo.StreetNumber,
                StreetName = addressInfo.StreetName,
                City = addressInfo.City,
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                County = addressInfo.County,
                StreetType = addressInfo.StreetType,
                Subdivision = addressInfo.Subdivision,
            };
        }
    }
}
