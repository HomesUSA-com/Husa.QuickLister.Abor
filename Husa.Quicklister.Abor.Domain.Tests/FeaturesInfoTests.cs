namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions.XML;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;
    using Husa.Xml.Api.Contracts.Response;
    using Xunit;

    public class FeaturesInfoTests
    {
        [Fact]
        public void UpdateFromXmlDescriptionIsDifferentUpdatesPropertyDescription()
        {
            // Arrange
            var fields = new StubSaleFeature
            {
                PropertyDescription = "Old Description",
            };
            var xmlListing = new XmlListingDetailResponse
            {
                Description = "New <br> Description with . extra . text",
            };

            // Act
            fields.UpdateFromXml(xmlListing);

            // Assert
            Assert.NotNull(fields.PropertyDescription);
            Assert.Contains("New", fields.PropertyDescription);
            Assert.DoesNotContain("<br>", fields.PropertyDescription);
        }

        [Fact]
        public void UpdateFromXml_DescriptionIsSame_NoChange()
        {
            // Arrange
            var fields = new StubSaleFeature
            {
                PropertyDescription = "Same. Text",
            };
            var xmlListing = new XmlListingDetailResponse
            {
                Description = "Same Text",
            };

            // Act
            fields.UpdateFromXml(xmlListing);

            // Assert
            Assert.Equal("Same. Text", fields.PropertyDescription);
        }

        [Fact]
        public void UpdateFromXml_NullOrEmptyDescription_NoChange()
        {
            // Arrange
            var fields = new StubSaleFeature
            {
                PropertyDescription = "Some existing value",
            };
            var xmlListing = new XmlListingDetailResponse
            {
                Description = null,
            };

            // Act
            fields.UpdateFromXml(xmlListing);

            // Assert
            Assert.Equal("Some existing value", fields.PropertyDescription);
        }

        [Fact]
        public void CleanPropertyDescription_ForComparisonOnly_RemovesSpacesAndDots()
        {
            // Arrange
            var fields = new StubSaleFeature
            {
                PropertyDescription = "Should be replaced",
            };
            var xmlListing = new XmlListingDetailResponse
            {
                Description = "Hello . .  world .. \n  with <br> spaces..",
            };

            // Act
            fields.UpdateFromXml(xmlListing);

            // Assert
            Assert.NotEqual("Should be replaced", fields.PropertyDescription);
            Assert.DoesNotContain("<br>", fields.PropertyDescription);
            Assert.Contains("Hello", fields.PropertyDescription);
            Assert.Contains("..", fields.PropertyDescription);
        }

        private sealed class StubSaleFeature : IProvideSaleFeature
        {
            public string PropertyDescription { get; set; }
            public bool IsAIGeneratedPropertyDescription { get; set; }
            public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<RestrictionsDescription> RestrictionsDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<Disclosures> Disclosures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<DocumentsAvailable> DocumentsAvailable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<UtilitiesDescription> UtilitiesDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<WaterSource> WaterSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<WaterSewer> WaterSewer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<HeatingSystem> HeatSystem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<CoolingSystem> CoolingSystem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<Appliances> Appliances { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int? GarageSpaces { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<GarageDescription> GarageDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<LaundryLocation> LaundryLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<InteriorFeatures> InteriorFeatures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int? Fireplaces { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<FireplaceDescription> FireplaceDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<Flooring> Floors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<SecurityFeatures> SecurityFeatures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<WindowFeatures> WindowFeatures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<Foundation> Foundation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<RoofDescription> RoofDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<Fencing> Fencing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<ConstructionMaterials> ConstructionMaterials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<View> View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ICollection<ExteriorFeatures> ExteriorFeatures { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }
    }
}
