namespace Husa.Quicklister.Abor.Crosscutting.Tests.LotListings
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class LotTestProvider
    {
        public static LotListing GetListingEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            MarketStatuses? marketStatuses = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listing = new LotListing(
                marketStatuses ?? MarketStatuses.Active,
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                listingCompanyId,
                Faker.Company.Name(),
                Faker.Enum.Random<Counties>(),
                communityId ?? Guid.NewGuid(),
                true)
            {
                Id = listingId ?? Guid.NewGuid(),
            };

            return listing;
        }

        public static LotListing GetFullListingEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            MarketStatuses? marketStatuses = null,
            LotListing lotListing = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listing = lotListing ?? new LotListing(
                marketStatuses ?? MarketStatuses.Active,
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                listingCompanyId,
                Faker.Company.Name(),
                Faker.Enum.Random<Counties>(),
                communityId ?? Guid.NewGuid(),
                true)
            {
                Id = listingId ?? Guid.NewGuid(),
                AddressInfo = new()
                {
                    StreetDirPrefix = StreetDirPrefix.East,
                    StreetDirSuffix = StreetDirPrefix.SouthEast,
                },
                FeaturesInfo = new()
                {
                    NeighborhoodAmenities = new List<NeighborhoodAmenities>()
                    {
                        NeighborhoodAmenities.Clubhouse,
                    },
                    Disclosures = new List<Disclosures>()
                    {
                        Disclosures.CorporateListing,
                    },
                    DistanceToWaterAccess = DistanceToWaterAccess.OneTwoMiles,
                    DocumentsAvailable = new List<DocumentsAvailable>()
                    {
                        DocumentsAvailable.AerialPhotos,
                    },
                    ExteriorFeatures = new List<ExteriorFeatures>()
                    {
                        ExteriorFeatures.Balcony,
                    },
                    Fencing = new List<Fencing>()
                    {
                        Fencing.BackYard,
                    },
                    GroundWaterConservDistric = false,
                    HorseAmenities = new List<HorseAmenities>()
                    {
                        HorseAmenities.Arena,
                    },
                    MineralsFeatures = new List<Minerals>()
                    {
                        Minerals.SeeRemarks,
                    },
                    OtherStructures = new List<OtherStructures>()
                    {
                        OtherStructures.AirplaneHangar,
                    },
                    RestrictionsDescription = new List<RestrictionsDescription>() { RestrictionsDescription.Adult55 },
                    RoadSurface = new List<RoadSurface>() { RoadSurface.AlleyPaved },
                    UtilitiesDescription = new List<UtilitiesDescription>() { UtilitiesDescription.AboveGroundUtilities },
                    View = new List<View>() { View.Bridges },
                    WaterfrontFeatures = new List<WaterfrontFeatures>() { WaterfrontFeatures.CanalFront },
                    WaterSewer = new List<WaterSewer>() { WaterSewer.EngineeredSeptic },
                    WaterSource = new List<WaterSource>() { WaterSource.MUD },
                },
                FinancialInfo = new()
                {
                    AcceptableFinancing = new List<AcceptableFinancing>() { AcceptableFinancing.Cash },
                    AgentBonusAmount = (decimal?)1.22,
                    AgentBonusAmountType = Quicklister.Extensions.Domain.Enums.CommissionType.Amount,
                    BillingFrequency = BillingFrequency.Annually,
                    BonusExpirationDate = DateTime.UtcNow,
                    BuyersAgentCommission = (decimal?)1.25,
                    BuyersAgentCommissionType = Quicklister.Extensions.Domain.Enums.CommissionType.Amount,
                    EstimatedTax = 12,
                    HasAgentBonus = true,
                    HasBonusWithAmount = true,
                    HasBuyerIncentive = true,
                    HasHoa = false,
                    HoaFee = (decimal?)12.2,
                    HoaIncludes = new List<HoaIncludes>() { HoaIncludes.Cable },
                    HoaName = "Name",
                    HOARequirement = HoaRequirement.Mandatory,
                    LandTitleEvidence = LandTitleEvidence.Buyer,
                    PreferredTitleCompany = "Title",
                    TaxAssesedValue = 12,
                    TaxExemptions = new List<TaxExemptions>() { TaxExemptions.Disability },
                    TaxRate = (decimal?)12.2,
                    TaxYear = 2020,
                },
                PropertyInfo = new()
                {
                    FemaFloodPlain = new List<FemaFloodPlain>() { FemaFloodPlain.Partial },
                    Latitude = (decimal?)12.3,
                    LegalDescription = "LegalDescription",
                    Longitude = (decimal?)12.2,
                    LotDescription = new List<LotDescription>() { LotDescription.BackYard },
                    LotDimension = "Dimension",
                    LotSize = "12",
                    MlsArea = MlsArea.TenN,
                    PropCondition = new List<PropCondition>() { PropCondition.ToBeBuilt },
                    PropertyType = PropertySubType.Condominium,
                    TaxBlock = "TaxBlock",
                    TaxId = "TaxId",
                    TaxLot = "TaxLot",
                    CommercialAllowed = true,
                    LiveStock = false,
                    NumberOfPonds = 1000,
                    NumberOfWells = 1000,
                    PropertySubType = PropertySubTypeLots.SingleLot,
                    SoilType = new List<SoilType>() { SoilType.BlackLand },
                    SurfaceWater = true,
                    TypeOfHomeAllowed = new List<TypeOfHomeAllowed>() { TypeOfHomeAllowed.ApprovalRequired },
                    AlsoListedAs = 123512,
                    BuilderRestrictions = false,
                },
                SchoolsInfo = new()
                {
                    ElementarySchool = ElementarySchool.Academy,
                    HighSchool = HighSchool.Academy,
                    MiddleSchool = MiddleSchool.AlterLearning,
                    OtherElementarySchool = "OtherElementarySchool",
                    OtherHighSchool = "OtherHighSchool",
                    OtherMiddleSchool = "OtherMiddleSchool",
                    SchoolDistrict = SchoolDistrict.Academy,
                },
                ShowingInfo = new()
                {
                    ApptPhone = "1235",
                    Directions = "Directions",
                    OwnerName = "OwnerName",
                    PublicRemarks = "Public Remarks",
                    ShowingContactType = new List<ShowingContactType>() { ShowingContactType.Agent },
                    ShowingInstructions = "Showing Instructions",
                    ShowingRequirements = new List<ShowingRequirements>() { ShowingRequirements.AgentOrOwnerPresent },
                    ShowingServicePhone = "1233",
                },
            };
            return listing;
        }

        public static LotListingDto GetLotListingDto() => new()
        {
            MlsStatus = MarketStatuses.Active,
            ExpirationDate = DateTime.UtcNow,
            FeaturesInfo = new(),
            FinancialInfo = new(),
            ShowingInfo = new(),
            SchoolsInfo = new(),
            PropertyInfo = new(),
            AddressInfo = new(),
        };
    }
}
