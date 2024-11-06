namespace Husa.Quicklister.Abor.Crosscutting.Tests.Community
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class CommunityTestProvider
    {
        public static CommunitySale GetCommunityEntity(Guid? communityId = null, Guid? companyId = null)
        {
            return new CommunitySale(
                companyId ?? Guid.NewGuid(),
                Faker.Lorem.GetFirstWord(),
                Faker.Company.Name())
            {
                Id = communityId ?? Guid.NewGuid(),
                Property = new()
                {
                    City = Faker.Enum.Random<Cities>(),
                    County = Faker.Enum.Random<Counties>(),
                },
                Utilities = new()
                {
                    NeighborhoodAmenities = new NeighborhoodAmenities[] { Faker.Enum.Random<NeighborhoodAmenities>() },
                },
                Financial = new(),
                SaleOffice = new(),
                EmailLead = new(),
                Showing = new(),
                SchoolsInfo = new(),
                JsonImportStatus = Quicklister.Extensions.Domain.Enums.Json.JsonImportStatus.NotFromJson,
            };
        }
    }
}
