namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public static class ScrapedListingTestProvider
    {
        public static ScrapedListing GetScrapedListingEntity(
            Guid? listingId = null,
            string builderName = "myTestBuilder",
            decimal listPrice = 100000,
            int price = 100000)
        {
            var scrapedListingId = listingId ?? Guid.NewGuid();
            var listing = new ScrapedListingValueObject()
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
                ListPrice = listPrice,
                Price = price,
                ListDate = DateTime.UtcNow,
                Refreshed = null,
                Comment = null,
            };
            return new ScrapedListing(listing)
            {
                Id = scrapedListingId,
            };
        }
    }
}
