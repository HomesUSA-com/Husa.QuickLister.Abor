namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public static class ListingTestProvider
    {
        public static void SetSysModifiedOnForTesting(this SaleListing entity, DateTime value)
        {
            typeof(SaleListing)
                .GetProperty("SysModifiedOn")
                .SetValue(entity, value);
        }

        public static SaleProperty GetSalePropertyEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            Guid? planId = null,
            MarketStatuses? marketStatuses = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listingPlanId = planId ?? Guid.NewGuid();
            var saleProperty = new SaleProperty(
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.RandomNumber.Next(1, 10).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                Faker.Enum.Random<Counties>(),
                DateTime.Now,
                listingCompanyId,
                Faker.Company.Name(),
                communityId ?? Guid.NewGuid(),
                listingPlanId)
            {
                Id = listingId ?? Guid.NewGuid(),
            };
            saleProperty.Plan = new Plan(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
            {
                Id = listingPlanId,
            };

            return saleProperty;
        }

        public static SaleListing GetListingEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            Guid? planId = null,
            MarketStatuses? marketStatuses = null,
            bool createCommunity = false,
            string mlsNumber = null,
            string ownerName = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listingPlanId = planId ?? Guid.NewGuid();
            var listing = new SaleListing(
                marketStatuses ?? MarketStatuses.Active,
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.RandomNumber.Next(1, 10).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                Faker.Enum.Random<Counties>(),
                DateTime.Now,
                listingCompanyId,
                ownerName ?? Faker.Company.Name(),
                communityId ?? Guid.NewGuid(),
                listingPlanId,
                Faker.Boolean.Random())
            {
                Id = listingId ?? Guid.NewGuid(),
                MlsNumber = mlsNumber,
            };
            listing.SaleProperty.Plan = new Plan(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
            {
                Id = listingPlanId,
            };
            listing.SaleProperty.PropertyInfo.TaxLot = "TaxLot";
            listing.SaleProperty.AddressInfo.StreetType = StreetType.ALY;

            if (createCommunity)
            {
                listing.SaleProperty.Community = new CommunitySale(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
                {
                    ProfileInfo = new ProfileInfo(Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord()),
                };
            }

            listing.SetSysModifiedOnForTesting(DateTime.UtcNow.AddDays(-1));
            return listing;
        }

        public static SaleListing GetListingEntity(
            ListType listType,
            Guid? planId = null,
            Guid? communityId = null,
            Guid? companyId = null,
            MarketStatuses mlsStatus = MarketStatuses.Active)
        {
            var listingInfo = new ListingValueObject()
            {
                ListType = listType,
                MlsStatus = mlsStatus,
            };
            var statusFieldsInfo = new ListingStatusFieldsInfo();
            var salePropertyInfo = new SalePropertyValueObject()
            {
                OwnerName = Faker.Company.Name(),
                PlanId = planId ?? Guid.NewGuid(),
                CommunityId = communityId ?? Guid.NewGuid(),
                PropertyInfo = new PropertyInfo(),
                AddressInfo = new SaleAddressInfo()
                {
                    StreetName = Faker.Address.StreetName(),
                    StreetNumber = Faker.RandomNumber.Next(1, 100).ToString(),
                    UnitNumber = Faker.RandomNumber.Next(1, 10).ToString(),
                    City = Faker.Enum.Random<Cities>(),
                    State = Faker.Enum.Random<States>(),
                    ZipCode = Faker.Address.ZipCode()[..5],
                    County = Faker.Enum.Random<Counties>(),
                },
                FeaturesInfo = new FeaturesInfo()
                {
                    WaterfrontFeatures = new List<WaterfrontFeatures> { WaterfrontFeatures.None },
                },
                SchoolsInfo = new SchoolsInfo(),
                ShowingInfo = new ShowingInfo(),
                SpacesDimensionsInfo = new SpacesDimensionsInfo(),
                FinancialInfo = new FinancialInfo(),
            };
            var listing = new SaleListing(
                listingInfo,
                statusFieldsInfo,
                salePropertyInfo,
                companyId ?? Guid.NewGuid(),
                false);
            listing.UpdateTrackValues(Guid.NewGuid());
            listing.SetSysModifiedOnForTesting(DateTime.UtcNow.AddDays(-1));
            return listing;
        }

        public static ListingSaleDetailRequest GetListingSaleDetailRequest() => new()
        {
            StatusFieldsInfo = new(),
            SaleProperty = new()
            {
                AddressInfo = new()
                {
                    StreetName = Faker.Address.StreetName(),
                    StreetNumber = Faker.RandomNumber.Next(1, 100).ToString(),
                    City = Faker.Enum.Random<Cities>(),
                    ZipCode = Faker.RandomNumber.Next(10000, 99999).ToString(),
                    State = Faker.Enum.Random<States>(),
                    County = Faker.Enum.Random<Counties>(),
                    Subdivision = "subdivision name",
                },
                PropertyInfo = new(),
                ShowingInfo = new(),
                SchoolsInfo = new(),
                FeaturesInfo = new()
                {
                    NeighborhoodAmenities = new[] { Faker.Enum.Random<NeighborhoodAmenities>() },
                },
                FinancialInfo = new(),
                SpacesDimensionsInfo = new()
                {
                    StoriesTotal = Faker.Enum.Random<Stories>(),
                },
            },
        };

        public static FeaturesInfo GetFeaturesInfo() => new()
        {
            GarageSpaces = 1,
            ParkingTotal = 1,
            Fireplaces = 0,
            HomeFaces = HomeFaces.West,
            PropertyDescription = "PropertyDescription",
            InteriorFeatures = new[] { InteriorFeatures.MultipleDiningAreas },
            DistanceToWaterAccess = DistanceToWaterAccess.SeeRemarks,
            NeighborhoodAmenities = new[] { NeighborhoodAmenities.GameRoom },
            RestrictionsDescription = new[] { RestrictionsDescription.Environmental },
            Disclosures = new[] { Disclosures.HomeProtectionPlan },
            DocumentsAvailable = new[] { DocumentsAvailable.BuildingPlans },
            UtilitiesDescription = new[] { UtilitiesDescription.Solar },
            WaterSource = new[] { WaterSource.Public },
            WaterSewer = new[] { WaterSewer.AerobicSeptic },
            HeatSystem = new[] { HeatingSystem.Fireplace },
            CoolingSystem = new[] { CoolingSystem.CeilingFan },
            Appliances = new[] { Appliances.SeeRemarks },
            GarageDescription = new[] { GarageDescription.Tandem },
            LaundryLocation = new[] { LaundryLocation.Kitchen },
            Floors = new[] { Flooring.Carpet },
            SecurityFeatures = new[] { SecurityFeatures.SecuritySystem },
            WindowFeatures = new[] { WindowFeatures.Blinds },
            Foundation = new[] { Foundation.Slab },
            RoofDescription = new[] { RoofDescription.Shingle },
            Fencing = new[] { Fencing.Block },
            ConstructionMaterials = new[] { ConstructionMaterials.BlownInInsulation },
            PatioAndPorchFeatures = new[] { PatioAndPorchFeatures.Deck },
            View = new[] { View.None },
            ExteriorFeatures = new[] { ExteriorFeatures.Balcony },
            GuestAccommodationsDescription = new[] { GuestAccommodationsDescription.None },
            WaterfrontFeatures = new[] { WaterfrontFeatures.LakeFront },
            WaterBodyName = WaterBodyName.LakeBastrop,
        };

        public static SalePropertyDetailDto GetSalePropertyDetailDto()
        {
            return new()
            {
                SpacesDimensionsInfo = new()
                {
                    StoriesTotal = Stories.MultiLevel,
                    SqFtTotal = 1235,
                    DiningAreasTotal = 4,
                    FullBathsTotal = 3,
                    HalfBathsTotal = 1,
                    LivingAreasTotal = 1,
                    MainLevelBedroomTotal = 1,
                    OtherLevelsBedroomTotal = 0,
                },
                FeaturesInfo = new()
                {
                    PropertyDescription = "PropertyDescription",
                    InteriorFeatures = new[] { InteriorFeatures.MultipleDiningAreas },
                    GarageSpaces = 1,
                    ParkingTotal = 1,
                    Fireplaces = 0,
                    HomeFaces = HomeFaces.West,
                    DistanceToWaterAccess = DistanceToWaterAccess.SeeRemarks,
                    NeighborhoodAmenities = new[] { NeighborhoodAmenities.GameRoom },
                    RestrictionsDescription = new[] { RestrictionsDescription.Environmental },
                    UtilitiesDescription = new[] { UtilitiesDescription.Solar },
                    WaterSource = new[] { WaterSource.Public },
                    WaterSewer = new[] { WaterSewer.AerobicSeptic },
                    HeatSystem = new[] { HeatingSystem.Fireplace },
                    CoolingSystem = new[] { CoolingSystem.CeilingFan },
                    Appliances = new[] { Appliances.SeeRemarks },
                    GarageDescription = new[] { GarageDescription.Tandem },
                    LaundryLocation = new[] { LaundryLocation.Kitchen },
                    Floors = new[] { Flooring.Carpet },
                    SecurityFeatures = new[] { SecurityFeatures.SecuritySystem },
                    WindowFeatures = new[] { WindowFeatures.Blinds },
                    Foundation = new[] { Foundation.Slab },
                    RoofDescription = new[] { RoofDescription.Shingle },
                    Fencing = new[] { Fencing.Block },
                    ConstructionMaterials = new[] { ConstructionMaterials.BlownInInsulation },
                    PatioAndPorchFeatures = new[] { PatioAndPorchFeatures.Deck },
                    View = new[] { View.None },
                    ExteriorFeatures = new[] { ExteriorFeatures.Balcony },
                    GuestAccommodationsDescription = new[] { GuestAccommodationsDescription.None },
                    WaterfrontFeatures = new[] { WaterfrontFeatures.None },
                },
                FinancialInfo = new()
                {
                    HOARequirement = HoaRequirement.Mandatory,
                    BuyersAgentCommission = 5,
                    TaxRate = 5,
                    TaxYear = 2023,
                    AcceptableFinancing = new[] { AcceptableFinancing.SeeRemarks },
                    TaxExemptions = new[] { TaxExemptions.Wildlife },
                },
                ShowingInfo = new()
                {
                    ShowingInstructions = "Call salesperson or come to the model home at 1234 Sample Trail.",
                    LockBoxType = LockBoxType.Combo,
                    ShowingRequirements = new[] { ShowingRequirements.ShowingService },
                },
                SchoolsInfo = new()
                {
                    SchoolDistrict = SchoolDistrict.Holland,
                    MiddleSchool = MiddleSchool.Holland,
                    HighSchool = HighSchool.Holland,
                    ElementarySchool = ElementarySchool.Holland,
                },
                PropertyInfo = new()
                {
                    ConstructionCompletionDate = new DateTime(DateTime.UtcNow.Year, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc),
                    ConstructionStage = ConstructionStage.Complete,
                    ConstructionStartYear = 2023,
                    PropertyType = PropertySubType.Condominium,
                    TaxId = "TaxId",
                    TaxLot = "TaxLot",
                    LotDescription = new[] { LotDescription.BackYard },
                },
                AddressInfo = new()
                {
                    County = Counties.Bee,
                    StreetType = StreetType.RDS,
                },
                SalePropertyInfo = new(),
                OpenHouses = new OpenHouseDto[] { new() },
                Rooms = new RoomDto[] { new() },
            };
        }
    }
}
