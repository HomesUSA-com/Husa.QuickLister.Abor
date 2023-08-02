namespace Husa.Quicklister.Abor.Data.Documents.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Extensions.Data.Documents.Extensions;
    using Models = Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;

    public static class ListingRequestRecordQueryExtensions
    {
        public static ListingRequestSalePropertyQueryResult ToProjectionListingSaleRequestSalePropertyQueryResult<T>(this T saleProperty)
            where T : SalePropertyRecord => new()
            {
                FeaturesInfo = saleProperty.FeaturesInfo.ToProjectionFeatures(),
                SalePropertyInfo = saleProperty.ToProjectionSaleProperty(),
                AddressInfo = saleProperty.AddressInfo.ToProjectionAddressInfo(),
                SpacesDimensionsInfo = saleProperty.SpacesDimensionsInfo.ToProjectionSpacesDimensions(),
                SchoolsInfo = saleProperty.SchoolsInfo.ToProjectionSchools(),
                PropertyInfo = saleProperty.PropertyInfo.ToProjectionPropertyInfo(),
                FinancialInfo = saleProperty.FinancialInfo.ToProjectionFinancial(),
                ShowingInfo = saleProperty.ShowingInfo.ToProjectionShowing(),
                Rooms = saleProperty.Rooms.ToProjectionRooms(),
                OpenHouses = saleProperty.OpenHouses.ToProjectionOpenHouses(),
                Hoas = saleProperty.ListingSaleHoas.ToProjectionHoas(),
            };

        public static AddressInfoQueryResult ToProjectionAddressInfo<T>(this T addressInfo)
            where T : AddressRecord => new()
            {
                StreetName = addressInfo.StreetName,
                StreetNumber = addressInfo.StreetNumber,
                Subdivision = addressInfo.Subdivision,
                City = addressInfo.City,
                County = addressInfo.County,
                Block = addressInfo.Block,
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                LotNum = addressInfo.LotNum,
            };

        public static Models.SalePropertyQueryResult ToProjectionSaleProperty<T>(this T saleProperty)
            where T : SalePropertyRecord => new()
            {
                CommunityId = saleProperty.CommunityId,
                CompanyId = saleProperty.CompanyId,
                OwnerName = saleProperty.OwnerName,
                PlanId = saleProperty.PlanId,
            };

        public static SpacesDimensionsInfoQueryResult ToProjectionSpacesDimensions<T>(this T spacesDimensions)
            where T : SpacesDimensionsRecord => new()
            {
                SpecialtyRooms = spacesDimensions.SpecialtyRooms,
                SqFtSource = spacesDimensions.SqFtSource,
                OtherParking = spacesDimensions.OtherParking,
                TypeCategory = spacesDimensions.TypeCategory,

                StoriesTotal = spacesDimensions.StoriesTotal,
                SqFtTotal = spacesDimensions.SqFtTotal,
                DiningAreasTotal = spacesDimensions.DiningAreasTotal,
                MainLevelBedroomTotal = spacesDimensions.MainLevelBedroomTotal,
                OtherLevelsBedroomTotal = spacesDimensions.OtherLevelsBedroomTotal,
                HalfBathsTotal = spacesDimensions.HalfBathsTotal,
                FullBathsTotal = spacesDimensions.FullBathsTotal,
                LivingAreasTotal = spacesDimensions.LivingAreasTotal,
            };

        public static Models.SchoolsInfoQueryResult ToProjectionSchools<T>(this T schools)
            where T : SchoolRecord => new()
            {
                SchoolDistrict = schools.SchoolDistrict,
                ElementarySchool = schools.ElementarySchool,
                HighSchool = schools.HighSchool,
                MiddleSchool = schools.MiddleSchool,
            };

        public static FeaturesInfoQueryResult ToProjectionFeatures<T>(this T features)
            where T : FeaturesRecord => new()
            {
                FireplaceDescription = features.FireplaceDescription,
                PropertyDescription = features.PropertyDescription,
                Fireplaces = features.Fireplaces,
                RoofDescription = features.RoofDescription,
                Accessibility = features.Accessibility,
                SupplierElectricity = features.SupplierElectricity,
                SupplierGarbage = features.SupplierGarbage,
                SupplierGas = features.SupplierGas,
                SupplierOther = features.SupplierOther,
                SupplierSewer = features.SupplierSewer,
                SupplierWater = features.SupplierWater,
                CoolingSystem = features.CoolingSystem,
                HeatSystem = features.HeatSystem,
                HousingStyle = features.HousingStyle,
                WaterSewer = features.WaterSewer,
                EnergyFeatures = features.EnergyFeatures,
                Exterior = features.Exterior,
                ExteriorFeatures = features.ExteriorFeatures,
                Floors = features.Floors,
                Foundation = features.Foundation,
                GreenCertification = features.GreenCertification,
                GreenFeatures = features.GreenFeatures,
                HasAccessibility = features.HasAccessibility,
                HasPrivatePool = features.HasPrivatePool,
                HeatingFuel = features.HeatingFuel,
                HomeFaces = features.HomeFaces,
                Inclusions = features.Inclusions,
                LotImprovements = features.LotImprovements,
                NeighborhoodAmenities = features.NeighborhoodAmenities,
                PrivatePool = features.PrivatePool,
                WindowCoverings = features.WindowCoverings,
                IsNewConstruction = features.IsNewConstruction,
            };

        public static ShowingInfoQueryResult ToProjectionShowing<T>(this T showing)
           where T : ShowingRecord => new()
           {
               Directions = showing.Directions,
               Showing = showing.Showing,
               AgentListApptPhone = showing.AgentListApptPhone,
               AgentPrivateRemarks = showing.AgentPrivateRemarks,
               AltPhoneCommunity = showing.AltPhoneCommunity,
               RealtorContactEmail = showing.RealtorContactEmail,
               EnableOpenHouses = showing.EnableOpenHouses,
               OpenHousesAgree = showing.OpenHousesAgree,
               ShowOpenHousesPending = showing.ShowOpenHousesPending,
           };

        public static FinancialInfoQueryResult ToProjectionFinancial<T>(this T financial)
            where T : FinancialRecord => new()
            {
                AgentBonusAmount = financial.AgentBonusAmount,
                AgentBonusAmountType = financial.AgentBonusAmountType,
                BonusExpirationDate = financial.BonusExpirationDate,
                BuyersAgentCommission = financial.BuyersAgentCommission,
                BuyersAgentCommissionType = financial.BuyersAgentCommissionType,
                HasAgentBonus = financial.HasAgentBonus,
                HasBonusWithAmount = financial.HasBonusWithAmount,
                HasBuyerIncentive = financial.HasBuyerIncentive,
                HasMultipleHOA = financial.HasMultipleHOA,
                HOARequirement = financial.HOARequirement,
                IsMultipleTaxed = financial.IsMultipleTaxed,
                NumHOA = financial.NumHOA,
                ProposedTerms = financial.ProposedTerms,
                TaxRate = financial.TaxRate,
                TaxYear = financial.TaxYear,
                TitleCompany = financial.TitleCompany,
            };

        public static PropertyInfoQueryResult ToProjectionPropertyInfo<T>(this T property)
           where T : PropertyRecord => new()
           {
               ConstructionCompletionDate = property.ConstructionCompletionDate,
               LegalDescription = property.LegalDescription,
               LotDescription = property.LotDescription,
               ConstructionStage = property.ConstructionStage,
               ConstructionStartYear = property.ConstructionStartYear,
               LotSize = property.LotSize,
               IsXmlManaged = property.IsXmlManaged,
               LotDimension = property.LotDimension,
               Latitude = property.Latitude,
               Longitude = property.Longitude,
               MapscoGrid = property.MapscoGrid,
               MlsArea = property.MlsArea,
               Occupancy = property.Occupancy,
               TaxId = property.TaxId,
               UpdateGeocodes = property.UpdateGeocodes,
           };

        public static ListingRequestStatusFieldsQueryResult ToProjectionListingSaleRequestStatusFieldsQueryResult<T>(this T statusField)
            where T : StatusFieldsRecord
        {
            if (statusField == null)
            {
                return new();
            }

            return new()
            {
                ContractDate = statusField.ContractDate,
                ExpiredDateOption = statusField.ExpiredDateOption,
                SellerConcessionDescription = statusField.SellerConcessionDescription,
                ContingencyInfo = statusField.ContingencyInfo,
                HasBuyerAgent = statusField.HasBuyerAgent,
                KickOutInformation = statusField.KickOutInformation,
                HowSold = statusField.HowSold,
                SaleTerms2nd = statusField.SaleTerms2nd,
                AgentId = statusField.AgentId,
                BackOnMarketDate = statusField.BackOnMarketDate,
                CancelDate = statusField.CancelDate,
                ClosedDate = statusField.ClosedDate,
                CancelledOption = statusField.CancelledOption,
                EstimatedClosedDate = statusField.EstimatedClosedDate,
                OffMarketDate = statusField.OffMarketDate,
                PendingDate = statusField.PendingDate,
                CancelledReason = statusField.CancelledReason,
                ClosePrice = statusField.ClosePrice,
                SellConcess = statusField.SellConcess,
                SellPoints = statusField.SellPoints,
            };
        }

        public static ListingRequestPublishInfoQueryResult ToProjectionListingRequestPublishInfoQueryResult<T>(this T publishInfo)
            where T : PublishFieldsRecord
        {
            if (publishInfo == null)
            {
                return new();
            }

            return new()
            {
                PublishType = publishInfo.PublishType,
                PublishDate = publishInfo.PublishDate,
                PublishStatus = publishInfo.PublishStatus,
                PublishUser = publishInfo.PublishUser,
            };
        }

        public static IEnumerable<RoomQueryResult> ToProjectionRooms<T>(this IEnumerable<T> rooms)
            where T : RoomRecord => rooms.Select(room => new RoomQueryResult
            {
                Id = room.Id,
                Level = room.Level,
                RoomType = room.RoomType,
                IsDeleted = room.IsDeleted,
            });

        public static IEnumerable<HoaQueryResult> ToProjectionHoas<T>(this IEnumerable<T> hoas)
            where T : HoaRecord => hoas.Select(hoa => new HoaQueryResult
            {
                Id = hoa.Id,
                Fee = hoa.Fee,
                BillingFrequency = hoa.BillingFrequency,
                ContactPhone = hoa.ContactPhone,
                Name = hoa.Name,
                TransferFee = hoa.TransferFee,
                Website = hoa.Website,
                IsDeleted = hoa.IsDeleted,
            });
    }
}
