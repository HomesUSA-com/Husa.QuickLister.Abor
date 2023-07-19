namespace Husa.Quicklister.Abor.Domain.Tests
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;
    using Xunit;

    public class AddressInfoTests
    {
        [Theory]
        [InlineData("Tx")]
        [InlineData("tX")]
        [InlineData("TX")]
        [InlineData("tx")]
        [InlineData("Texas")]
        [InlineData("texas")]
        [InlineData("TEXAS")]
        [InlineData("North Carolina")]
        [InlineData("north carolina")]
        [InlineData("NORTH CAROLINA")]
        [InlineData("NC")]
        [InlineData("nc")]
        public void AddressInfoConvertsStatesCorrectly(string state)
        {
            // Arrange
            var xmlListing = new XmlListingDetailResponse
            {
                StreetNum = "1234",
                StreetName = "sample",
                City = "North Shore Estates",
                State = state,
                Zip = "75034",
                County = "Armstrong",
                Lot = "109",
                Block = "D",
                Subdivision = "Fake Subdivision",
            };

            var addressInfo = new AddressInfo
            {
                StreetNumber = "1234",
                StreetName = "sample",
                City = Cities.CiboloCity,
                State = States.Tennessee,
                ZipCode = "75034",
                County = Counties.Anderson,
                LotNum = "109",
                Block = "D",
                Subdivision = "Fake Subdivision",
            };

            // Act
            var result = AddressInfo.ImportFromXml(xmlListing, addressInfo);

            // Assert
            Assert.NotNull(result);
        }
    }
}
