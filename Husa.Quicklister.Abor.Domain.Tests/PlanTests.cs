namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class PlanTests
    {
        [Fact]
        public void ImportFromListing_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var bathsFull = 2;
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.SpacesDimensionsInfo.BathsFull = bathsFull;

            var companyId = Guid.NewGuid();
            var plan = new Plan(companyId, "testName", "testOwnerName");
            plan.BasePlan.BathsFull = 1;

            // Act
            plan.ImportFromListing(listing);

            // Assert
            Assert.Equal(listing.SaleProperty.SpacesDimensionsInfo.BathsFull, plan.BasePlan.BathsFull);
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
            Assert.Equal(stories, basePlan.Stories);
        }
    }
}
