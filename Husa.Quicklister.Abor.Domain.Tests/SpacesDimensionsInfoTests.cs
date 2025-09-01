namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Extensions.XML;
    using Husa.Xml.Api.Contracts.Response;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class SpacesDimensionsInfoTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateSqftoFromXmlSuccess(bool manageSqft)
        {
            var xmlListing = new XmlListingDetailResponse()
            {
               Sqft = 12,
            };

            var spacesDimensionsInfo = new SpacesDimensionsInfo()
            {
                SqFtTotal = null,
            };

            // Arrange
            var xmlLsitng = new XmlListingDetailResponse
            {
                Latitude = default(decimal),
                Longitude = default(decimal),
                LegalDescLot = "legal desc",
            };

            // Act
            spacesDimensionsInfo.UpdateFromXml(xmlListing);

            // Assert
            var result = manageSqft ? xmlLsitng.Sqft : spacesDimensionsInfo.SqFtTotal;
            Assert.Equal(result, spacesDimensionsInfo.SqFtTotal);
        }
    }
}
