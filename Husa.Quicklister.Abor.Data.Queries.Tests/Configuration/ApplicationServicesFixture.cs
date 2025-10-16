namespace Husa.Quicklister.Abor.Data.Queries.Tests.Configuration
{
    using System;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;

    public class ApplicationServicesFixture
    {
        public readonly DbContextOptions<ApplicationDbContext> DbOptions;

        public ApplicationServicesFixture()
        {
            this.Mapper = Bootstrapper.ConfigureAutoMapper();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            this.DbOptions = optionsBuilder.Options;
        }

        public IMapper Mapper { get; }

        public TDbContext GetInMemoryDbContext<TDbContext>()
            where TDbContext : ApplicationDbContext
        {
            var dbContext = (TDbContext)Activator.CreateInstance(
                typeof(TDbContext), this.DbOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
