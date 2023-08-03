namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Extensions.Data.Queries.Projections;

    public static class SaleLisitngEntityQueryExtensions
    {
        public static ICollection<RoomQueryResult> ToProjectionRooms<T>(this ICollection<T> rooms)
            where T : Room
        {
            var roomsCollection = new List<RoomQueryResult>();
            foreach (var r in rooms)
            {
                var room = new RoomQueryResult
                {
                    Id = r.Id,
                    Level = r.Level,
                    RoomType = r.RoomType,
                    IsDeleted = r.IsDeleted,
                };

                roomsCollection.Add(room);
            }

            return roomsCollection;
        }

        public static ICollection<HoaQueryResult> ToProjectionHoas<T>(this ICollection<T> hoas)
            where T : Hoa
        {
            var hoaCollection = new List<HoaQueryResult>();
            foreach (var h in hoas)
            {
                var hoa = new HoaQueryResult
                {
                    Id = h.Id,
                    Fee = h.Fee,
                    BillingFrequency = h.BillingFrequency,
                    ContactPhone = h.ContactPhone,
                    Name = h.Name,
                    TransferFee = h.TransferFee,
                    Website = h.Website,
                    IsDeleted = h.IsDeleted,
                };

                hoaCollection.Add(hoa);
            }

            return hoaCollection;
        }

        public static FeaturesQueryResult ToProjectionFeatures<T>(this T features)
            where T : FeaturesInfo
        {
            return new()
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
            };
        }

        public static Models.ListingShowingQueryResult ToProjectionShowing<T>(this T showing)
            where T : ShowingInfo
        {
            return new()
            {
                Directions = showing.Directions,
                ShowingInstructions = showing.ShowingInstructions,
                ContactPhone = showing.ContactPhone,
                AgentPrivateRemarks = showing.AgentPrivateRemarks,
                OccupantPhone = showing.OccupantPhone,
                EnableOpenHouses = showing.EnableOpenHouses,
                OpenHousesAgree = showing.OpenHousesAgree,
                ShowOpenHousesPending = showing.ShowOpenHousesPending,
            };
        }

        public static SpacesDimensionsQueryResult ToProjectionSpacesDimensions<T>(this T spacesDimensions)
            where T : SpacesDimensionsInfo
        {
            return new()
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
        }

        public static FinancialQueryResult ToProjectionSpacesFinancial<T>(this T financial)
            where T : FinancialInfo
        {
            return new()
            {
                AgentBonusAmount = financial.AgentBonusAmount,
                AgentBonusAmountType = financial.AgentBonusAmountType,
                BonusExpirationDate = financial.BonusExpirationDate,
                BuyersAgentCommission = financial.BuyersAgentCommission,
                BuyersAgentCommissionType = financial.BuyersAgentCommissionType,
                HasAgentBonus = financial.HasAgentBonus,
                HasBonusWithAmount = financial.HasBonusWithAmount,
                HasBuyerIncentive = financial.HasBuyerIncentive,
                HOARequirement = financial.HOARequirement,
                TaxRate = financial.TaxRate,
                TaxYear = financial.TaxYear,
                TitleCompany = financial.TitleCompany,
            };
        }

        public static SchoolsInfoQueryResult ToProjectionSchools<T>(this T schools)
            where T : SchoolsInfo
        {
            return new()
            {
                SchoolDistrict = schools.SchoolDistrict,
                ElementarySchool = schools.ElementarySchool,
                HighSchool = schools.HighSchool,
                MiddleSchool = schools.MiddleSchool,
            };
        }

        public static ListingSalePublishInfoQueryResult ToProjectionPublishInfo<T>(this T publishInfo)
            where T : PublishInfo
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

        public static AddressQueryResult ToProjectionAddressInfo<T>(this T addressInfo)
            where T : AddressInfo
        {
            return new()
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
        }

        public static PropertyInfoQueryResult ToProjectionPropertyInfo<T>(this T propertyInfo)
            where T : PropertyInfo
        {
            return new()
            {
                ConstructionCompletionDate = propertyInfo.ConstructionCompletionDate,
                LegalDescription = propertyInfo.LegalDescription,
                LotDescription = propertyInfo.LotDescription,
                ConstructionStage = propertyInfo.ConstructionStage,
                ConstructionStartYear = propertyInfo.ConstructionStartYear,
                LotSize = propertyInfo.LotSize,
                IsXmlManaged = propertyInfo.IsXmlManaged,
                LotDimension = propertyInfo.LotDimension,
                Latitude = propertyInfo.Latitude,
                Longitude = propertyInfo.Longitude,
                MapscoGrid = propertyInfo.MapscoGrid,
                MlsArea = propertyInfo.MlsArea,
                Occupancy = propertyInfo.Occupancy,
                TaxId = propertyInfo.TaxId,
                UpdateGeocodes = propertyInfo.UpdateGeocodes,
            };
        }

        public static SalePropertyDetailQueryResult ToProjection(this SaleProperty saleProperty)
        {
            return new()
            {
                SalePropertyInfo = saleProperty.ToProjectionSaleProperty(),
                AddressInfo = saleProperty.AddressInfo.ToProjectionAddressInfo(),
                SchoolsInfo = saleProperty.SchoolsInfo.ToProjectionSchools(),
                ShowingInfo = saleProperty.ShowingInfo.ToProjectionShowing(),
                FeaturesInfo = saleProperty.FeaturesInfo.ToProjectionFeatures(),
                PropertyInfo = saleProperty.PropertyInfo.ToProjectionPropertyInfo(),
                SpacesDimensionsInfo = saleProperty.SpacesDimensionsInfo.ToProjectionSpacesDimensions(),
                FinancialInfo = saleProperty.FinancialInfo.ToProjectionSpacesFinancial(),
                OpenHouses = saleProperty.OpenHouses.ToProjectionOpenHouses(),
                Rooms = saleProperty.Rooms.ToProjectionRooms(),
                Hoas = saleProperty.ListingSaleHoas.ToProjectionHoas(),
            };
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
                MlsArea = property.MlsArea,
                Subdivision = property.Subdivision,
                ZipCode = property.ZipCode,
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
                Directions = showing.Directions,
                LockBoxType = showing.LockBoxType,
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
                HasBuyerIncentive = community.Financial.HasBuyerIncentive,
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
                    Inclusions = utilities.Inclusions,
                    Floors = utilities.Floors,
                    ExteriorFeatures = utilities.ExteriorFeatures,
                    RoofDescription = utilities.RoofDescription,
                    Foundation = utilities.Foundation,
                    HeatSystem = utilities.HeatSystem,
                    CoolingSystem = utilities.CoolingSystem,
                    GreenCertification = utilities.GreenCertification,
                    EnergyFeatures = utilities.EnergyFeatures,
                    GreenFeatures = utilities.GreenFeatures,
                    WaterSewer = utilities.WaterSewer,
                    SupplierElectricity = utilities.SupplierElectricity,
                    SupplierWater = utilities.SupplierWater,
                    SupplierSewer = utilities.SupplierSewer,
                    SupplierGarbage = utilities.SupplierGarbage,
                    SupplierGas = utilities.SupplierGas,
                    SupplierOther = utilities.SupplierOther,
                    HeatingFuel = utilities.HeatingFuel,
                    Exterior = utilities.Exterior,
                    Accessibility = utilities.Accessibility,
                    FireplaceDescription = utilities.FireplaceDescription,
                    Fireplaces = utilities.Fireplaces,
                    HasAccessibility = utilities.HasAccessibility,
                    SpecialtyRooms = utilities.SpecialtyRooms,
                };
        }

        public static SchoolsInfoQueryResult ToProjectionSchoolsInfo(this SchoolsInfo schools)
        {
            if (schools == null)
            {
                return new();
            }

            return new()
            {
                ElementarySchool = schools.ElementarySchool,
                HighSchool = schools.HighSchool,
                MiddleSchool = schools.MiddleSchool,
                SchoolDistrict = schools.SchoolDistrict,
                OtherElementarySchool = schools.OtherElementarySchool,
                OtherHighSchool = schools.OtherHighSchool,
                OtherMiddleSchool = schools.OtherMiddleSchool,
            };
        }

        public static EmailLeadQueryResult ToProjectionEmailLead(this EmailLead emailLeads)
        {
            if (emailLeads == null)
            {
                return new();
            }

            return new()
            {
                EmailLeadPrincipal = emailLeads.EmailLeadPrincipal,
                EmailLeadSecondary = emailLeads.EmailLeadSecondary,
                EmailLeadOther = emailLeads.EmailLeadOther,
            };
        }

        public static ListingSaleStatusFieldQueryResult ToProjectionStatusFieldsInfo(this ListingSaleStatusFieldsInfo statusFieldsInfo)
        {
            if (statusFieldsInfo == null)
            {
                return new();
            }

            return new()
            {
                CancelledOption = statusFieldsInfo.CancelledOption,
                CancelDate = statusFieldsInfo.CancelDate,
                CancelledReason = statusFieldsInfo.CancelledReason,
                AgentId = statusFieldsInfo.AgentId,
                HasBuyerAgent = statusFieldsInfo.HasBuyerAgent,
                BackOnMarketDate = statusFieldsInfo.BackOnMarketDate,
                ClosedDate = statusFieldsInfo.ClosedDate,
                ClosePrice = statusFieldsInfo.ClosePrice,
                ContingencyInfo = statusFieldsInfo.ContingencyInfo,
                SaleTerms2nd = statusFieldsInfo.SaleTerms2nd,
                ContractDate = statusFieldsInfo.ContractDate,
                EstimatedClosedDate = statusFieldsInfo.EstimatedClosedDate,
                ExpiredDateOption = statusFieldsInfo.ExpiredDateOption,
                KickOutInformation = statusFieldsInfo.KickOutInformation,
                HowSold = statusFieldsInfo.HowSold,
                SellPoints = statusFieldsInfo.SellPoints,
                SellConcess = statusFieldsInfo.SellConcess,
                SellerConcessionDescription = statusFieldsInfo.SellerConcessionDescription,
                OffMarketDate = statusFieldsInfo.OffMarketDate,
                PendingDate = statusFieldsInfo.PendingDate,
            };
        }
    }
}
