namespace Husa.Quicklister.Abor.Data.Queries.Tests.Seeders
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;

    public static class ApplicationDbContextTestFactory
    {
        public static (T DbContext, DbContextOptionsBuilder<T> Options) DbContextFactory<T>()
            where T : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var dbContext = (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return (dbContext, optionsBuilder);
        }

        public static CommunitySale CommunityFactory(Guid communityId, Guid companyId)
        {
            return new CommunitySale(companyId, Faker.Address.StreetAddress(), Faker.Name.FullName())
            {
                Id = communityId,
            };
        }

        public static ShowingTimeContact ShowingTimeContactFactory(Guid contactId, Guid companyId) => new()
        {
            Id = contactId,
            CompanyId = companyId,
            Email = Faker.Internet.Email(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            MobilePhone = Faker.Phone.Number(),
            OfficePhone = Faker.Phone.Number(),
        };

        public static CommunityShowingTimeContact CommunityShowingTimeContactFactory(Guid contactId, Guid communityId, int order) => new()
        {
            ContactId = contactId,
            ScopeId = communityId,
            Order = order,
        };
    }
}
