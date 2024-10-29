namespace Husa.Quicklister.Abor.Data.Queries.Extensions.Sale
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Extensions.Data.Queries.Projections;
    using OpenHousesQueryResult = Husa.Quicklister.Abor.Data.Queries.Models.OpenHousesQueryResult;

    public static class SaleLisitngEntityQueryExtensions
    {
        public static FeaturesQueryResult ToProjectionFeatures<T>(this T features)
            where T : FeaturesInfo
        {
            return new()
            {
                NeighborhoodAmenities = features.NeighborhoodAmenities,
                RestrictionsDescription = features.RestrictionsDescription,
                Disclosures = features.Disclosures,
                DocumentsAvailable = features.DocumentsAvailable,
                UtilitiesDescription = features.UtilitiesDescription,
                WaterSource = features.WaterSource,
                WaterSewer = features.WaterSewer,
                HeatSystem = features.HeatSystem,
                CoolingSystem = features.CoolingSystem,
                Appliances = features.Appliances,
                GarageSpaces = features.GarageSpaces,
                GarageDescription = features.GarageDescription,
                LaundryLocation = features.LaundryLocation,
                InteriorFeatures = features.InteriorFeatures,
                Fireplaces = features.Fireplaces,
                FireplaceDescription = features.FireplaceDescription,
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
                WaterfrontFeatures = features.WaterfrontFeatures,
                WaterBodyName = features.WaterBodyName,
                DistanceToWaterAccess = features.DistanceToWaterAccess,
                UnitStyle = features.UnitStyle,
                GuestAccommodationsDescription = features.GuestAccommodationsDescription,
                GuestBedroomsTotal = features.GuestBedroomsTotal,
                GuestFullBathsTotal = features.GuestFullBathsTotal,
                GuestHalfBathsTotal = features.GuestHalfBathsTotal,
                PropertyDescription = features.PropertyDescription,
                IsNewConstruction = features.IsNewConstruction,
            };
        }

        public static Models.ListingShowingQueryResult ToProjectionShowing<T>(this T showing)
            where T : ShowingInfo
        {
            return new()
            {
                OccupantPhone = showing.OccupantPhone,
                ContactPhone = showing.ContactPhone,
                AgentPrivateRemarks = showing.AgentPrivateRemarks,
                AgentPrivateRemarksAdditional = showing.AgentPrivateRemarksAdditional,
                LockBoxSerialNumber = showing.LockBoxSerialNumber,
                ShowingInstructions = showing.ShowingInstructions,
                ShowingRequirements = showing.ShowingRequirements,
                LockBoxType = showing.LockBoxType,
                RealtorContactEmail = showing.RealtorContactEmail,
                Directions = showing.Directions,
                EnableOpenHouses = showing.EnableOpenHouses,
                ShowOpenHousesPending = showing.ShowOpenHousesPending,
                OwnerName = showing.OwnerName,
            };
        }

        public static SpacesDimensionsQueryResult ToProjectionSpacesDimensions<T>(this T spacesDimensions)
            where T : SpacesDimensionsInfo
        {
            return new()
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
        }

        public static FinancialQueryResult ToProjectionFinancial<T>(this T financial)
            where T : FinancialInfo
        {
            return new()
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
            };
        }

        public static SalePropertyQueryResult ToProjectionSaleProperty<T>(this T saleProperty)
            where T : SaleProperty
        {
            return new()
            {
                OwnerName = saleProperty.OwnerName,
                CompanyId = saleProperty.CompanyId,
                CommunityId = saleProperty.CommunityId,
                PlanId = saleProperty.PlanId,
                PlanName = saleProperty.Plan?.BasePlan?.Name,
            };
        }

        public static PropertyInfoQueryResult ToProjectionPropertyInfo<T>(this T propertyInfo)
            where T : PropertyInfo
        {
            return new()
            {
                ConstructionCompletionDate = propertyInfo.ConstructionCompletionDate,
                ConstructionStage = propertyInfo.ConstructionStage,
                ConstructionStartYear = propertyInfo.ConstructionStartYear,
                LegalDescription = propertyInfo.LegalDescription,
                TaxId = propertyInfo.TaxId,
                TaxLot = propertyInfo.TaxLot,
                MlsArea = propertyInfo.MlsArea,
                LotDimension = propertyInfo.LotDimension,
                LotSize = propertyInfo.LotSize,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType,
                UpdateGeocodes = propertyInfo.UpdateGeocodes,
                Latitude = propertyInfo.Latitude,
                Longitude = propertyInfo.Longitude,
                IsXmlManaged = propertyInfo.IsXmlManaged,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
            };
        }

        public static SalePropertyDetailQueryResult ToProjection(this SaleProperty saleProperty)
        {
            return new()
            {
                SalePropertyInfo = saleProperty.ToProjectionSaleProperty(),
                AddressInfo = saleProperty.AddressInfo.ToProjectionSaleAddressInfo(),
                SchoolsInfo = saleProperty.SchoolsInfo.ToProjectionSchoolsInfo(),
                ShowingInfo = saleProperty.ShowingInfo.ToProjectionShowing(),
                FeaturesInfo = saleProperty.FeaturesInfo.ToProjectionFeatures(),
                PropertyInfo = saleProperty.PropertyInfo.ToProjectionPropertyInfo(),
                SpacesDimensionsInfo = saleProperty.SpacesDimensionsInfo.ToProjectionSpacesDimensions(),
                FinancialInfo = saleProperty.FinancialInfo.ToProjectionFinancial(),
                OpenHouses = saleProperty.OpenHouses.ToProjectionOpenHouses(),
                Rooms = saleProperty.Rooms.ToProjectionRooms(),
            };
        }

        public static List<OpenHousesQueryResult> ToProjectionOpenHouses(this ICollection<SaleListingOpenHouse> openHouses)
        {
            var openHouseCollection = new List<OpenHousesQueryResult>();

            if (openHouses == null)
            {
                return openHouseCollection;
            }

            foreach (var openH in openHouses)
            {
                if (openH == null)
                {
                    continue;
                }

                var openHouse = new OpenHousesQueryResult
                {
                    Type = openH.Type,
                    EndTime = openH.EndTime,
                    StartTime = openH.StartTime,
                    Refreshments = openH.Refreshments,
                };

                openHouseCollection.Add(openHouse);
            }

            return openHouseCollection;
        }

        public static PropertyQueryResult ToProjectionProperty(this Property property)
        {
            if (property == null)
            {
                return new();
            }

            return new()
            {
                City = property.City,
                County = property.County,
                ZipCode = property.ZipCode,
                Subdivision = property.Subdivision,
                LotSize = property.LotSize,
                MlsArea = property.MlsArea,
                ConstructionStage = property.ConstructionStage,
                LotDimension = property.LotDimension,
                LotDescription = property.LotDescription,
                PropertyType = property.PropertyType,
            };
        }

        public static ShowingQueryResult ToProjectionCommunityShowing(this CommunityShowingInfo showing)
        {
            if (showing == null)
            {
                return new();
            }

            return new()
            {
                OccupantPhone = showing.OccupantPhone,
                ContactPhone = showing.ContactPhone,
                ShowingInstructions = showing.ShowingInstructions,
                ShowingRequirements = showing.ShowingRequirements,
                RealtorContactEmail = showing.RealtorContactEmail,
                Directions = showing.Directions,
                LockBoxType = showing.LockBoxType,
                OwnerName = showing.OwnerName,
            };
        }

        public static ProfileQueryResult ToProjectionProfile<T>(this T community)
            where T : CommunitySale
        {
            return new()
            {
                Name = community.ProfileInfo.Name,
                OwnerName = community.ProfileInfo.OwnerName,
                OfficePhone = community.ProfileInfo.OfficePhone,
                BackupPhone = community.ProfileInfo.BackupPhone,
                Fax = community.ProfileInfo.Fax,
                UseLatLong = community.ProfileInfo.UseLatLong,
                Latitude = community.ProfileInfo.Latitude,
                Longitude = community.ProfileInfo.Longitude,
                EmailMailViolationsWarnings = community.ProfileInfo.EmailMailViolationsWarnings,
                SalesOffice = community.SaleOffice.ToProjectionSaleOffice(),
                EmailLead = community.EmailLead.ToProjectionEmailLead(),
            };
        }

        public static FinancialSchoolsQueryResult ToProjectionFinancialSchools<T>(this T community)
            where T : CommunitySale
        {
            return new()
            {
                TaxRate = community.Financial.TaxRate,
                TitleCompany = community.Financial.TitleCompany,
                AcceptableFinancing = community.Financial.AcceptableFinancing,
                TaxExemptions = community.Financial.TaxExemptions,
                HoaIncludes = community.Financial.HoaIncludes,
                HasHoa = community.Financial.HasHoa,
                HoaName = community.Financial.HoaName,
                HoaFee = community.Financial.HoaFee,
                BillingFrequency = community.Financial.BillingFrequency,
                HOARequirement = community.Financial.HOARequirement,
                BuyersAgentCommission = community.Financial.BuyersAgentCommission,
                BuyersAgentCommissionType = community.Financial.BuyersAgentCommissionType,
                HasAgentBonus = community.Financial.HasAgentBonus,
                HasBonusWithAmount = community.Financial.HasBonusWithAmount,
                AgentBonusAmount = community.Financial.AgentBonusAmount,
                AgentBonusAmountType = community.Financial.AgentBonusAmountType,
                BonusExpirationDate = community.Financial.BonusExpirationDate,
                Schools = community.SchoolsInfo?.ToProjectionSchoolsInfo(),
            };
        }

        public static SalesOfficeQueryResult ToProjectionSaleOffice(this CommunitySaleOffice saleOffice)
        {
            if (saleOffice == null)
            {
                return new();
            }

            return new()
            {
                IsSalesOffice = saleOffice.IsSalesOffice,
                SalesOfficeCity = saleOffice.SalesOfficeCity,
                StreetName = saleOffice.StreetName,
                StreetNumber = saleOffice.StreetNumber,
                StreetSuffix = saleOffice.StreetSuffix,
                SalesOfficeZip = saleOffice.SalesOfficeZip,
            };
        }

        public static UtilitiesQueryResult ToProjectionUtilities(this Utilities utilities)
        {
            if (utilities == null)
            {
                return new();
            }

            return new()
            {
                NeighborhoodAmenities = utilities.NeighborhoodAmenities,
                RestrictionsDescription = utilities.RestrictionsDescription,
                Disclosures = utilities.Disclosures,
                DocumentsAvailable = utilities.DocumentsAvailable,
                UtilitiesDescription = utilities.UtilitiesDescription,
                WaterSource = utilities.WaterSource,
                WaterSewer = utilities.WaterSewer,
                HeatSystem = utilities.HeatSystem,
                CoolingSystem = utilities.CoolingSystem,
                Appliances = utilities.Appliances,
                GarageSpaces = utilities.GarageSpaces,
                GarageDescription = utilities.GarageDescription,
                LaundryLocation = utilities.LaundryLocation,
                InteriorFeatures = utilities.InteriorFeatures,
                Fireplaces = utilities.Fireplaces,
                FireplaceDescription = utilities.FireplaceDescription,
                Floors = utilities.Floors,
                SecurityFeatures = utilities.SecurityFeatures,
                WindowFeatures = utilities.WindowFeatures,
                Foundation = utilities.Foundation,
                RoofDescription = utilities.RoofDescription,
                Fencing = utilities.Fencing,
                ConstructionMaterials = utilities.ConstructionMaterials,
                PatioAndPorchFeatures = utilities.PatioAndPorchFeatures,
                View = utilities.View,
                ExteriorFeatures = utilities.ExteriorFeatures,
            };
        }

        private static SaleAddressQueryResult ToProjectionSaleAddressInfo(this SaleAddressInfo addressInfo)
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
                UnitNumber = addressInfo.UnitNumber,
            };
        }
    }
}
