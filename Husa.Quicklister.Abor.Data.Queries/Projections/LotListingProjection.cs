namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public static class LotListingProjection
    {
        public static Expression<Func<LotListing, LotListingQueryResult>> ProjectToLotListingQueryResult => listing => new LotListingQueryResult
        {
            Id = listing.Id,
            MlsNumber = listing.MlsNumber,
            StreetName = listing.AddressInfo.StreetName,
            StreetNum = listing.AddressInfo.StreetNumber,
            StreetType = listing.AddressInfo.StreetType,
            City = listing.AddressInfo.City,
            County = listing.AddressInfo.County,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            MarketModifiedOn = listing.MarketModifiedOn,
            MlsStatus = listing.MlsStatus,
            SysModifiedOn = listing.SysModifiedOn,
            SysModifiedBy = listing.SysModifiedBy,
            Subdivision = listing.AddressInfo.Subdivision,
            ZipCode = listing.AddressInfo.ZipCode,
            OwnerName = listing.OwnerName,
            MarketCode = MarketCode.Austin,
            SysCreatedOn = listing.SysCreatedOn,
            SysCreatedBy = listing.SysCreatedBy,
            CommunityId = listing.CommunityId,
            UnitNumber = listing.AddressInfo.UnitNumber,
        };

        public static Expression<Func<LotListing, LotListingQueryDetailResult>> ProjectToLotListingQueryDetail => listing => new LotListingQueryDetailResult
        {
            Id = listing.Id,
            ExpirationDate = listing.ExpirationDate,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            ListType = listing.ListType,
            MlsNumber = listing.MlsNumber,
            MlsStatus = listing.MlsStatus,
            LockedBy = listing.LockedBy,
            LockedOn = listing.LockedOn,
            LockedStatus = listing.LockedStatus,
            SysCreatedBy = listing.SysCreatedBy,
            SysCreatedOn = listing.SysCreatedOn,
            SysModifiedBy = listing.SysModifiedBy,
            SysModifiedOn = listing.SysModifiedOn,
            IsPhotosDeclined = listing.IsPhotosDeclined,
            IsManuallyManaged = listing.IsManuallyManaged,
            XmlListingId = listing.XmlListingId,
            XmlDiscrepancyListingId = listing.XmlDiscrepancyListingId,
            OwnerName = listing.OwnerName,
            CommunityId = listing.CommunityId,
            CompanyId = listing.CompanyId,
            AddressInfo = listing.AddressInfo.ToProjectionAddressInfo(),
            SchoolsInfo = listing.SchoolsInfo.ToProjectionSchools(),
            ShowingInfo = listing.ShowingInfo.ToProjectionShowing(),
            FeaturesInfo = listing.FeaturesInfo.ToProjectionFeatures(),
            PropertyInfo = listing.PropertyInfo.ToProjectionPropertyInfo(),
            FinancialInfo = listing.FinancialInfo.ToProjectionFinancial(),
        };

        public static LotPropertyQueryResult ToProjectionPropertyInfo<T>(this T propertyInfo)
            where T : LotPropertyInfo
        {
            return new()
            {
                MlsArea = propertyInfo.MlsArea,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
            };
        }

        public static LotFinancialQueryResult ToProjectionFinancial<T>(this T financial)
            where T : LotFinancialInfo
        {
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
            where T : LotFeaturesInfo
        {
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
            where T : LotShowingInfo
        {
            return new()
            {
                ShowingRequirements = showing.ShowingRequirements,
            };
        }
    }
}
