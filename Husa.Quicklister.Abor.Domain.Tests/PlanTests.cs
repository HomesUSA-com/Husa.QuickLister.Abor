namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Tests.Providers;
    using Moq;
    using Xunit;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class PlanTests
    {
        [Fact]
        public void ImportFromSaleProperty_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var bathsFull = 2;
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.SpacesDimensionsInfo.FullBathsTotal = bathsFull;

            var companyId = Guid.NewGuid();
            var plan = new Plan(companyId, "testName", "testOwnerName");
            plan.BasePlan.FullBathsTotal = 1;

            // Act
            plan.ImportFromSaleProperty(listing.SaleProperty);

            // Assert
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.FullBathsTotal, plan.BasePlan.FullBathsTotal);
        }

        [Theory]
        [InlineData(Stories.One, 1)]
        [InlineData(Stories.OnePointFive, 1.5)]
        [InlineData(Stories.Two, 2)]
        [InlineData(Stories.ThreePlus, 3)]
        [InlineData(Stories.ThreePlus, 4)]
        public void ImportFromXml_Success(Stories stories, decimal value)
        {
            // Arrange
            var companyName = Faker.Company.Name();
            var planXml = new XmlResponse.PlanResponse()
            {
                Stories = value,
            };

            // Act
            var basePlan = BasePlan.ImportFromXml(planXml, companyName);

            // Assert
            Assert.Equal(stories, basePlan.StoriesTotal);
        }

        [Fact]
        public void ImportFromListingIfFieldsAreEmpty()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.SpacesDimensionsInfo = TestModelProvider.GetSpacesDimensionsInfo();

            var companyId = Guid.NewGuid();
            var plan = new Plan(companyId, "testName", "testOwnerName");

            // Act
            plan.ImportFromSaleProperty(listing.SaleProperty, updateRooms: false, overwriteFieldsOnlyIfNull: true);

            // Assert
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.StoriesTotal, plan.BasePlan.StoriesTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.HalfBathsTotal, plan.BasePlan.HalfBathsTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.FullBathsTotal, plan.BasePlan.FullBathsTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.SqFtTotal, plan.BasePlan.SqFtTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.DiningAreasTotal, plan.BasePlan.DiningAreasTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.MainLevelBedroomTotal, plan.BasePlan.MainLevelBedroomTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.OtherLevelsBedroomTotal, plan.BasePlan.OtherLevelsBedroomTotal);
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.LivingAreasTotal, plan.BasePlan.LivingAreasTotal);
        }

        [Fact]
        public void ImportFromListingIfFieldsAreEmpty_FieldsNotEmpty()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.SpacesDimensionsInfo = TestModelProvider.GetSpacesDimensionsInfo();

            var companyId = Guid.NewGuid();
            var plan = new Plan(companyId, "testName", "testOwnerName");

            var planStories = Stories.Two;
            var bathsHalfTotal = 2;
            var bathsFullTotal = 3;
            var sqFtTotal = 2;
            var diningAreasTotal = 2;
            var mainLevelBedroomTotal = 2;
            var otherLevelsBedroomTotal = 3;
            var livingAreasTotal = 2;
            this.SetBasePlanSpaceDimensionsFields(
                basePlan: plan.BasePlan,
                planStories: planStories,
                bathsHalfTotal: bathsHalfTotal,
                bathsFullTotal: bathsFullTotal,
                sqFtTotal: sqFtTotal,
                diningAreasTotal: diningAreasTotal,
                mainLevelBedroomTotal: mainLevelBedroomTotal,
                otherLevelsBedroomTotal: otherLevelsBedroomTotal,
                livingAreasTotal: livingAreasTotal);

            // Act
            plan.ImportFromSaleProperty(listing.SaleProperty, updateRooms: false, overwriteFieldsOnlyIfNull: true);

            // Assert
            Assert.Equal(planStories, plan.BasePlan.StoriesTotal);
            Assert.Equal(bathsHalfTotal, plan.BasePlan.HalfBathsTotal);
            Assert.Equal(bathsFullTotal, plan.BasePlan.FullBathsTotal);
            Assert.Equal(sqFtTotal, plan.BasePlan.SqFtTotal);
            Assert.Equal(diningAreasTotal, plan.BasePlan.DiningAreasTotal);
            Assert.Equal(mainLevelBedroomTotal, plan.BasePlan.MainLevelBedroomTotal);
            Assert.Equal(otherLevelsBedroomTotal, plan.BasePlan.OtherLevelsBedroomTotal);
            Assert.Equal(livingAreasTotal, plan.BasePlan.LivingAreasTotal);
        }

        [Fact]
        public void GetActiveListingsInMarket()
        {
            var planId = Guid.NewGuid();
            var plan = new Mock<Plan>();
            plan.Setup(x => x.Id).Returns(planId);
            plan.Setup(x => x.ActiveListingsInMarketExpression).CallBase();
            var expression = plan.Object.ActiveListingsInMarketExpression;
            var listings = new[]
            {
                GetListinMock(Guid.NewGuid(), planId),
                GetListinMock(Guid.NewGuid(), planId),
                GetListinMock(Guid.NewGuid(), planId, MarketStatuses.Closed),
                GetListinMock(Guid.NewGuid(), Guid.NewGuid()),
            };
            var result = listings.Where(expression.Compile());
            Assert.Equal(2, result.Count());
            static Domain.Entities.Listing.SaleListing GetListinMock(Guid id, Guid planId, MarketStatuses? mlsStatus = null)
                => TestEntityProvider.GetSaleListing(id, planId: planId, mlsStatus: mlsStatus);
        }

        private void SetBasePlanSpaceDimensionsFields(
            BasePlan basePlan,
            Stories planStories,
            int bathsHalfTotal,
            int bathsFullTotal,
            int sqFtTotal,
            int diningAreasTotal,
            int mainLevelBedroomTotal,
            int otherLevelsBedroomTotal,
            int livingAreasTotal)
        {
            basePlan.StoriesTotal = planStories;
            basePlan.HalfBathsTotal = bathsHalfTotal;
            basePlan.FullBathsTotal = bathsFullTotal;
            basePlan.SqFtTotal = sqFtTotal;
            basePlan.DiningAreasTotal = diningAreasTotal;
            basePlan.MainLevelBedroomTotal = mainLevelBedroomTotal;
            basePlan.OtherLevelsBedroomTotal = otherLevelsBedroomTotal;
            basePlan.LivingAreasTotal = livingAreasTotal;
        }
    }
}
