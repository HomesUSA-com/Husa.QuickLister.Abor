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
            };

        public static AddressInfoQueryResult ToProjectionAddressInfo<T>(this T addressInfo)
            where T : AddressRecord => new()
            {
                StreetName = addressInfo.StreetName,
                StreetNumber = addressInfo.StreetNumber,
                Subdivision = addressInfo.Subdivision,
                City = addressInfo.City,
                County = addressInfo.County,
                UnitNumber = addressInfo.UnitNumber,
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                StreetType = addressInfo.StreetType,
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
                PropertyDescription = features.PropertyDescription,
                Fireplaces = features.Fireplaces,
                FireplaceDescription = features.FireplaceDescription,
                NeighborhoodAmenities = features.NeighborhoodAmenities,
                RestrictionsDescription = features.RestrictionsDescription,
                UtilitiesDescription = features.UtilitiesDescription,
                WaterSource = features.WaterSource,
                WaterSewer = features.WaterSewer,
                HeatSystem = features.HeatSystem,
                CoolingSystem = features.CoolingSystem,
                Appliances = features.Appliances,
                GarageSpaces = features.GarageSpaces,
                GarageDescription = features.GarageDescription,
                LaundryFeatures = features.LaundryFeatures,
                LaundryLocation = features.LaundryLocation,
                InteriorFeatures = features.InteriorFeatures,
                KitchenFeatures = features.KitchenFeatures,
                MasterBedroomFeatures = features.MasterBedroomFeatures,
                WaterAccessDescription = features.WaterAccessDescription,
                Floors = features.Floors,
                SecurityFeatures = features.SecurityFeatures,
                WindowFeatures = features.WindowFeatures,
                Foundation = features.Foundation,
                RoofDescription = features.RoofDescription,
                Fencing = features.Fencing,
                ConstructionMaterials = features.ConstructionMaterials,
                PatioAndPorchFeatures = features.PatioAndPorchFeatures,
                View = features.View,
                ExteriorFeatures = features.ExteriorFeatures,
                HomeFaces = features.HomeFaces,
                WaterBodyName = features.WaterBodyName,
                DistanceToWaterAccess = features.DistanceToWaterAccess,
                WaterfrontFeatures = features.WaterfrontFeatures,
                UnitStyle = features.UnitStyle,
                GuestAccommodationsDescription = features.GuestAccommodationsDescription,
                GuestBedroomsTotal = features.GuestBedroomsTotal,
                GuestFullBathsTotal = features.GuestFullBathsTotal,
                GuestHalfBathsTotal = features.GuestHalfBathsTotal,
                IsNewConstruction = features.IsNewConstruction,
            };

        public static ShowingInfoQueryResult ToProjectionShowing<T>(this T showing)
           where T : ShowingRecord => new()
           {
               Directions = showing.Directions,
               ShowingInstructions = showing.ShowingInstructions,
               ShowingRequirements = showing.ShowingRequirements,
               LockBoxType = showing.LockBoxType,
               ContactPhone = showing.ContactPhone,
               AgentPrivateRemarks = showing.AgentPrivateRemarks,
               OccupantPhone = showing.OccupantPhone,
               RealtorContactEmail = showing.RealtorContactEmail,
               EnableOpenHouses = showing.EnableOpenHouses,
               OpenHousesAgree = showing.OpenHousesAgree,
               ShowOpenHousesPending = showing.ShowOpenHousesPending,
           };

        public static FinancialInfoQueryResult ToProjectionFinancial<T>(this T financial)
            where T : FinancialRecord => new()
            {
                TaxYear = financial.TaxYear,
                TaxRate = financial.TaxRate,
                TitleCompany = financial.TitleCompany,
                AcceptableFinancing = financial.AcceptableFinancing,
                TaxExemptions = financial.TaxExemptions,
                HoaIncludes = financial.HoaIncludes,
                HasHoa = financial.HasHoa,
                HoaName = financial.HoaName,
                HoaFee = financial.HoaFee,
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
               MlsArea = property.MlsArea,
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
    }
}
