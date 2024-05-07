namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LotListingQueriesRepositoryTests
    {
        private readonly Mock<IUserContextProvider> userContextMock = new();
        private readonly Mock<ILogger<LotListingQueriesRepository>> loggerMock = new();
        private readonly Mock<IUserRepository> userQueriesRepositoryMock = new();

        [Fact]
        public async Task GetAsync_WithValidFilter_ReturnsDataSet()
        {
            // Arrange
            var queryFilter = new ListingQueryFilter
            {
                MlsStatus = new[] { MarketStatuses.Active },
                CommunityId = Guid.NewGuid(),
                MlsNumber = "123456",
            };

            this.SetupMlsAdmin();
            var listings = new List<LotListing>
            {
                new LotListing
                {
                    CommunityId = queryFilter.CommunityId.Value,
                    MlsNumber = queryFilter.MlsNumber,
                    MlsStatus = queryFilter.MlsStatus.First(),
                },
            };

            var community = CommunityTestProvider.GetCommunityEntity(queryFilter.CommunityId.Value);
            var sut = this.GetInMemoryRepository(listings, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetAsync(queryFilter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal(queryFilter.CommunityId, result.Data.First().CommunityId);
            Assert.Equal(queryFilter.MlsNumber, result.Data.First().MlsNumber);
            Assert.Equal(queryFilter.MlsStatus.First(), result.Data.First().MlsStatus);
        }

        [Fact]
        public async Task GetListing_WithValidListingId_ReturnsLotListingQueryDetailResult()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.SetupMlsAdmin();

            var lotListing = new LotListing(
                    MarketStatuses.Active,
                    Faker.Address.StreetName(),
                    Faker.RandomNumber.Next(1, 100).ToString(),
                    Faker.Enum.Random<Cities>(),
                    Husa.Extensions.Common.Enums.States.Texas,
                    Faker.Address.ZipCode()[..5],
                    Guid.NewGuid(),
                    Faker.Company.Name(),
                    Faker.Enum.Random<Counties>(),
                    Guid.NewGuid(),
                    true)
            {
                Id = listingId,
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
                    TaxExemptions = new List<TaxExemptions>() { TaxExemptions.Agricultural },
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
                    LotSize = 12,
                    MlsArea = MlsArea.TenN,
                    PropCondition = new List<PropCondition>() { PropCondition.ToBeBuilt },
                    PropertyType = PropertySubType.Condominium,
                    TaxBlock = "TaxBlock",
                    TaxId = "TaxId",
                    TaxLot = "TaxLot",
                    LiveStock = false,
                    NumberOfPonds = 1000,
                    NumberOfWells = 1000,
                    PropertySubType = PropertySubTypeLots.SingleLot,
                    SoilType = new List<SoilType>() { SoilType.BlackLand },
                    SurfaceWater = true,
                    TypeOfHomeAllowed = new List<TypeOfHomeAllowed>() { TypeOfHomeAllowed.ApprovalRequired },
                    CommercialAllowed = true,
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

            var community = CommunityTestProvider.GetCommunityEntity(lotListing.CommunityId.Value);
            var sut = this.GetInMemoryRepository(new[] { lotListing }, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
            Assert.Equal(lotListing.CommunityId, result.CommunityId);
            Assert.Equal(lotListing.MlsNumber, result.MlsNumber);
            Assert.Equal(lotListing.MlsStatus, result.MlsStatus);
            Assert.Equal(lotListing.LockedStatus, result.LockedStatus);
            Assert.Equal(lotListing.PropertyInfo.TaxId, result.PropertyInfo.TaxId);
            Assert.Equal(lotListing.PropertyInfo.TaxBlock, result.PropertyInfo.TaxBlock);
            Assert.Equal(lotListing.PropertyInfo.TaxLot, result.PropertyInfo.TaxLot);
            Assert.Equal(lotListing.PropertyInfo.Latitude, result.PropertyInfo.Latitude);
            Assert.Equal(lotListing.PropertyInfo.FemaFloodPlain, result.PropertyInfo.FemaFloodPlain);
            Assert.Equal(lotListing.PropertyInfo.LegalDescription, result.PropertyInfo.LegalDescription);
            Assert.Equal(lotListing.PropertyInfo.PropCondition, result.PropertyInfo.PropCondition);
            Assert.Equal(lotListing.PropertyInfo.PropertyType, result.PropertyInfo.PropertyType);
            Assert.Equal(lotListing.PropertyInfo.LotDimension, result.PropertyInfo.LotDimension);
            Assert.Equal(lotListing.SchoolsInfo.HighSchool, result.SchoolsInfo.HighSchool);
            Assert.Equal(lotListing.SchoolsInfo.MiddleSchool, result.SchoolsInfo.MiddleSchool);
            Assert.Equal(lotListing.SchoolsInfo.ElementarySchool, result.SchoolsInfo.ElementarySchool);
            Assert.Equal(lotListing.SchoolsInfo.OtherElementarySchool, result.SchoolsInfo.OtherElementarySchool);
            Assert.Equal(lotListing.SchoolsInfo.OtherMiddleSchool, result.SchoolsInfo.OtherMiddleSchool);
            Assert.Equal(lotListing.SchoolsInfo.OtherHighSchool, result.SchoolsInfo.OtherHighSchool);
            Assert.Equal(lotListing.FinancialInfo.HasHoa, result.FinancialInfo.HasHoa);
            Assert.Equal(lotListing.FinancialInfo.HoaName, result.FinancialInfo.HoaName);
            Assert.Equal(lotListing.FinancialInfo.HoaIncludes, result.FinancialInfo.HoaIncludes);
            Assert.Equal(lotListing.FinancialInfo.LandTitleEvidence, result.FinancialInfo.LandTitleEvidence);
            Assert.Equal(lotListing.FinancialInfo.PreferredTitleCompany, result.FinancialInfo.PreferredTitleCompany);
            Assert.Equal(lotListing.FinancialInfo.TaxExemptions, result.FinancialInfo.TaxExemptions);
            Assert.Equal(lotListing.FinancialInfo.TaxAssesedValue, result.FinancialInfo.TaxAssesedValue);
            Assert.Equal(lotListing.FeaturesInfo.NeighborhoodAmenities, result.FeaturesInfo.NeighborhoodAmenities);
            Assert.Equal(lotListing.FeaturesInfo.DocumentsAvailable, result.FeaturesInfo.DocumentsAvailable);
            Assert.Equal(lotListing.PropertyInfo.CommercialAllowed, result.PropertyInfo.CommercialAllowed);
            Assert.Equal(lotListing.FeaturesInfo.MineralsFeatures, result.FeaturesInfo.MineralsFeatures);
            Assert.Equal(lotListing.AddressInfo.StreetDirSuffix, result.AddressInfo.StreetDirSuffix);
        }

        [Fact]
        public async Task GetListing_WithEmptySections()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.SetupMlsAdmin();

            var lotListing = new LotListing
            {
                CommunityId = Guid.NewGuid(),
                Id = listingId,
                ShowingInfo = null,
                SchoolsInfo = null,
                FeaturesInfo = null,
            };

            var community = CommunityTestProvider.GetCommunityEntity(lotListing.CommunityId.Value);
            var sut = this.GetInMemoryRepository(new[] { lotListing }, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
        }

        private void SetupMlsAdmin()
        {
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContextMock.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }

        private LotListingQueriesRepository GetInMemoryRepository(IEnumerable<LotListing> listings, IEnumerable<CommunitySale> communities)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            foreach (var listing in listings)
            {
                dbContext.LotListing.Add(listing);
            }

            foreach (var community in communities)
            {
                dbContext.Community.Add(community);
            }

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);

            return new LotListingQueriesRepository(
                queriesDbContext,
                this.userContextMock.Object,
                this.loggerMock.Object,
                this.userQueriesRepositoryMock.Object);
        }
    }
}
