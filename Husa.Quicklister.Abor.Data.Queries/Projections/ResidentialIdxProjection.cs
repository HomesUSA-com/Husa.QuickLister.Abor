namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using AddressQueryResult = Husa.Quicklister.Extensions.Data.Queries.Models.AddressQueryResult;

    public static class ResidentialIdxProjection
    {
        public static Expression<Func<SaleListing, ResidentialIdxQueryResult>> ToResidentialIdxQueryResult => listing => new ResidentialIdxQueryResult
        {
            Id = listing.Id,
            XmlId = listing.XmlListingId,
            MlsNumber = listing.MlsNumber,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            MlsStatus = listing.MlsStatus.ToString(),
            MarketCode = MarketCode.Austin,
            Address = listing.SaleProperty.AddressInfo.ToAddressQueryResult(),
            Schools = listing.SaleProperty.SchoolsInfo.ToSchoolsQueryResult(),
            Financial = listing.SaleProperty.FinancialInfo.ToFinancialQueryResult(),
            Property = listing.SaleProperty.PropertyInfo.ToPropertyQueryResult(),
            SpacesDimensions = listing.SaleProperty.SpacesDimensionsInfo.ToSpacesDimensionsQueryResult(),
        };

        private static AddressQueryResult ToAddressQueryResult(this SaleAddressInfo addressInfo)
            => addressInfo == null
            ? new()
            : new()
            {
                StreetNumber = addressInfo.StreetNumber,
                StreetName = addressInfo.StreetName,
                City = addressInfo.City.ToString(),
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                County = addressInfo.County.ToString(),
                StreetType = addressInfo.StreetType.ToString(),
                Subdivision = addressInfo.Subdivision,
                UnitNumber = addressInfo.UnitNumber,
            };

        private static SchoolsIdxQueryResult ToSchoolsQueryResult(this SchoolsInfo info)
            => info == null
            ? new()
            : new()
            {
                SchoolDistrict = info.SchoolDistrict,
                MiddleSchool = info.MiddleSchool,
                ElementarySchool = info.ElementarySchool,
                HighSchool = info.HighSchool,
            };

        private static FinancialIdxQueryResult ToFinancialQueryResult(this FinancialInfo info)
            => info == null
            ? new()
            : new()
            {
                AcceptableFinancing = info.AcceptableFinancing,
                BuyersAgentCommission = info.BuyersAgentCommission,
                BuyersAgentCommissionType = info.BuyersAgentCommissionType,
                HasAgentBonus = info.HasAgentBonus,
                HasBonusWithAmount = info.HasBonusWithAmount,
                AgentBonusAmount = info.AgentBonusAmount,
                AgentBonusAmountType = info.AgentBonusAmountType,
                BonusExpirationDate = info.BonusExpirationDate,
            };

        private static PropertyIdxQueryResult ToPropertyQueryResult(this PropertyInfo info)
            => info == null
            ? new()
            : new()
            {
                ConstructionCompletionDate = info.ConstructionCompletionDate,
                ConstructionStage = info.ConstructionStage,
                ConstructionStartYear = info.ConstructionStartYear,
                LotSize = info.LotSize,
                PropertyType = info.PropertyType,
                Latitude = info.Latitude,
                Longitude = info.Longitude,
            };

        private static SpacesDimensionsIdxQueryResult ToSpacesDimensionsQueryResult(this SpacesDimensionsInfo info)
            => info == null
            ? new()
            : new()
            {
                SqFtTotal = info.SqFtTotal,
                NumBedrooms = info.MainLevelBedroomTotal,
                NumHalfBaths = info.HalfBathsTotal,
                NumFullBaths = info.FullBathsTotal,
                NumDiningAreas = info.DiningAreasTotal,
                NumLivingAreas = info.LivingAreasTotal,
            };
    }
}
