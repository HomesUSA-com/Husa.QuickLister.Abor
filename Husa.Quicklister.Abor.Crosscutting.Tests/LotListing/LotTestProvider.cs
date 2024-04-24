namespace Husa.Quicklister.Abor.Crosscutting.Tests.LotListings
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class LotTestProvider
    {
        public static LotListing GetListingEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            MarketStatuses? marketStatuses = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listing = new LotListing(
                marketStatuses ?? MarketStatuses.Active,
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                listingCompanyId,
                Faker.Company.Name(),
                Faker.Enum.Random<Counties>(),
                communityId ?? Guid.NewGuid(),
                true)
            {
                Id = listingId ?? Guid.NewGuid(),
            };

            return listing;
        }

        public static LotListingDto GetLotListingDto() => new()
        {
            MlsStatus = MarketStatuses.Active,
            ExpirationDate = DateTime.UtcNow,
            FeaturesInfo = new(),
            FinancialInfo = new(),
            ShowingInfo = new(),
            SchoolsInfo = new(),
            PropertyInfo = new(),
            AddressInfo = new(),
        };
    }
}
