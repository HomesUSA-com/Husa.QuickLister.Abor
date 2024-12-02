namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Xunit;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using DownloaderTrestleEnums = Husa.Downloader.CTX.Domain.Enums;

    public class OfficeTests
    {
        [Fact]
        public void ConstructorWithValidOfficeValueShouldInitializeCorrectly()
        {
            // Arrange
            var officeValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                Name = "Main Office",
                Address = "123 Main St",
                City = Cities.Adina,
                StateOrProvince = DownloaderTrestleEnums.StateOrProvince.TX,
                Zip = "75201",
                ZipExt = "1234",
                Phone = "555-555-1234",
                Status = OfficeStatus.Active,
                MarketModified = DateTimeOffset.UtcNow,
                Type = OfficeType.RealtorOffice,
            };

            // Act
            var office = new Office(officeValue);

            // Assert
            Assert.Equal(officeValue, office.OfficeValue);
        }

        [Fact]
        public void ConstructorWithoutParametersShouldInitializeEmptyOffice()
        {
            // Act
            var office = new Office();

            // Assert
            Assert.Null(office.OfficeValue);
        }

        [Fact]
        public void UpdateInformationWithNewerMarketModifiedShouldUpdateOfficeValue()
        {
            // Arrange
            var oldValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var newValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var office = new Office(oldValue);

            // Act
            office.UpdateInformation(newValue);

            // Assert
            Assert.Equal(newValue, office.OfficeValue);
        }

        [Fact]
        public void UpdateInformationWithOlderMarketModifiedShouldNotUpdateOfficeValue()
        {
            // Arrange
            var oldValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var newValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var office = new Office(oldValue);

            // Act
            office.UpdateInformation(newValue);

            // Assert
            Assert.Equal(oldValue, office.OfficeValue);
        }

        [Fact]
        public void UpdateInformationWithSameMarketModifiedShouldNotUpdateOfficeValue()
        {
            // Arrange
            var oldValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var newValue = new OfficeValueObject
            {
                MarketUniqueId = "123",
                MarketModified = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };

            var office = new Office(oldValue);

            // Act
            office.UpdateInformation(newValue);

            // Assert
            Assert.Equal(oldValue, office.OfficeValue);
        }
    }
}
