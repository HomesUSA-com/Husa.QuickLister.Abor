namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ScrapedListingTests
    {
        [Fact]
        public void MarketIsAustinSuccess()
        {
            // Arange && Act && Assert
            Assert.Equal(MarketCode.Austin, ScrapedListing.Market);
        }

        [Fact]
        public void CreateListingSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };

            // Act
            var sut = new ScrapedListing(scrapedListingValueObject);

            // Assert
            Assert.Equal(sut.ListingDetails, scrapedListingValueObject);
            Assert.Equal(MarketCode.Austin, ScrapedListing.Market);
        }

        [Fact]
        public void CreateListingNullObjectFails()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new ScrapedListing(null));
        }

        [Fact]
        public void CreateListingNegativePriceFails()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = -1,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };

            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ScrapedListing(scrapedListingValueObject));
        }

        [Fact]
        public void CreateListingNegativeListPriceFails()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = -1,
                Price = 120000,
                ListDate = listDate,
                Refreshed = DateTime.UtcNow,
                Comment = null,
                UnitNum = null,
            };

            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ScrapedListing(scrapedListingValueObject));
        }

        [Fact]
        public void CreateListingListPriceAndPriceAreZeroFails()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 0,
                Price = 0,
                ListDate = listDate,
                Refreshed = DateTime.UtcNow,
                Comment = null,
                UnitNum = null,
            };

            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ScrapedListing(scrapedListingValueObject));
        }

        [Fact]
        public void CreateListingSetNotInMLSCommentSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var expectedComment = "Not in MLS";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 0,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };

            // Act
            var sut = new ScrapedListing(scrapedListingValueObject);

            // Assert
            Assert.Equal(sut.ListingDetails.Comment, expectedComment);
        }

        [Fact]
        public void CreateListingSetNotInBuilderWebsiteCommentSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var expectedComment = "Not in Builder Website";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 0,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };

            // Act
            var sut = new ScrapedListing(scrapedListingValueObject);

            // Assert
            Assert.Equal(sut.ListingDetails.Comment, expectedComment);
        }

        [Fact]
        public void UpdateListingSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };
            var sut = new ScrapedListing(scrapedListingValueObject);
            var updatedListing = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 120000,
                Price = 120000,
                ListDate = listDate,
                Refreshed = DateTime.UtcNow,
                Comment = null,
                UnitNum = null,
            };

            // Act
            sut.UpdateInformation(updatedListing);

            // Assert
            Assert.NotEqual(sut.ListingDetails, scrapedListingValueObject);
        }

        [Fact]
        public void UpdateListingRefreshedEarlierThanListDateFails()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };
            var sut = new ScrapedListing(scrapedListingValueObject);
            var updatedListing = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 120000,
                Price = 120000,
                ListDate = listDate,
                Refreshed = listDate.AddHours(-1),
                Comment = null,
                UnitNum = null,
            };

            // Act && Assert
            Assert.Throws<ArgumentException>(() => sut.UpdateInformation(updatedListing));
        }

        [Fact]
        public void UpdateListingRefreshedLaterThanListDateSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };
            var sut = new ScrapedListing(scrapedListingValueObject);
            var updatedListing = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 120000,
                Price = 120000,
                ListDate = listDate,
                Refreshed = listDate.AddHours(1),
                Comment = null,
                UnitNum = null,
            };

            // Act
            sut.UpdateInformation(updatedListing);

            // Assert
            Assert.Equal(sut.ListingDetails, updatedListing);
        }

        [Fact]
        public void UpdateListingListingIsNullFails()
        {
            // Arange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var sut = new ScrapedListing(new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            });

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => sut.UpdateInformation(null));
        }

        [Fact]
        public void UpdateListingListingIsTheSameSuccess()
        {
            // Arrange
            var builderName = "MyTestBuilder";
            var listDate = DateTime.UtcNow;
            var scrapedListingValueObject = new ScrapedListingValueObject
            {
                OfficeName = Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                BuilderName = builderName,
                DOM = Faker.RandomNumber.Next(),
                UncleanBuilder = builderName,
                MlsNum = Faker.RandomNumber.Next(10000).ToString(),
                ListStatus = MarketStatuses.Active,
                Community = Faker.Address.UkCounty(),
                City = Faker.Address.City(),
                ListPrice = 100000,
                Price = 100000,
                ListDate = listDate,
                Refreshed = null,
                Comment = null,
                UnitNum = null,
            };
            var sut = new ScrapedListing(scrapedListingValueObject);

            // Act
            sut.UpdateInformation(scrapedListingValueObject);

            // Assert
            Assert.Same(sut.ListingDetails, scrapedListingValueObject);
        }
    }
}
