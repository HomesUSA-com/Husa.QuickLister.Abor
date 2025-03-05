namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Extensions.XML;
    using Husa.Xml.Api.Contracts.Response;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Tests")]
    public class PropertyInfoTests
    {
        [Theory]
        [InlineData("-1")]
        [InlineData("+1")]
        public void ImportFromPropertyInfoFromXmlSuccess(string offsetString)
        {
            int offset = int.Parse(offsetString);
            var dateInput = DateTime.Today.AddDays(offset).Date;
            var propertyInfo = new PropertyInfo();

            // Arrange
            var xmlLsitng = new XmlListingDetailResponse
            {
                Latitude = default(decimal),
                Longitude = default(decimal),
                Day = dateInput,
                LegalDescLot = "legal desc",
            };

            // Act
            var featuresClonned = PropertyInfo.ImportFromXml(xmlLsitng, propertyInfo);

            // Assert
            Assert.Equal(xmlLsitng.Day.Value.Date, featuresClonned.ConstructionCompletionDate.Value.Date);
            var constructionStage = xmlLsitng.Day.Value.Date > DateTime.UtcNow.Date ? Enums.Domain.ConstructionStage.Incomplete : Enums.Domain.ConstructionStage.Complete;
            Assert.Equal(featuresClonned.ConstructionStage, constructionStage);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("+1")]
        public void UpdateFromPropertyInfoFromXmlSuccess(string offsetString)
        {
            int offset = int.Parse(offsetString);
            var dateInput = DateTime.Today.AddDays(offset).Date;
            var xmlListing = new XmlListingDetailResponse()
            {
                Day = dateInput,
            };
            var propertyInfo = new PropertyInfo()
            {
                ConstructionCompletionDate = dateInput.AddDays(1),
            };

            // Arrange
            var xmlLsitng = new XmlListingDetailResponse
            {
                Latitude = default(decimal),
                Longitude = default(decimal),
                Day = dateInput,
                LegalDescLot = "legal desc",
            };

            // Act
            propertyInfo.UpdateFromXml(xmlListing);

            // Assert
            var constructionStage = xmlLsitng.Day.Value.Date > DateTime.UtcNow.Date ? Enums.Domain.ConstructionStage.Incomplete : Enums.Domain.ConstructionStage.Complete;
            Assert.Equal(propertyInfo.ConstructionStage, constructionStage);
        }
    }
}
