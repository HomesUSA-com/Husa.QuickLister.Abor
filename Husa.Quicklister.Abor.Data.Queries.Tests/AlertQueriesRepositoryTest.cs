namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Serilog;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]

    public class AlertQueriesRepositoryTest
    {
        private const string Connectionstring = "add-db-connection-string";

        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IPhotoServiceClient> photoService = new();
        private readonly Mock<IUserContextProvider> userContex = new();
        private readonly Mock<ILogger<AlertQueriesRepository>> logger = new();

        [Fact(Skip = "This test is only meant for debugging purposes and avoid launching the whole app to test a single query")]
        public async Task GetAlertsSuccess()
        {
            // Arrange
            Log.Logger = new LoggerConfiguration().WriteTo.Debug().CreateBootstrapLogger();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(Log.Logger);
            this.userContex.Setup(u => u.GetCurrentUser()).Returns(GetRealUser());
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(Connectionstring, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory);

            using var context = new ApplicationQueriesDbContext(optionsBuilder.Options);
            var sut = new AlertQueriesRepository(
                context,
                this.userQueriesRepository.Object,
                this.photoService.Object,
                this.logger.Object,
                this.userContex.Object);

            var alertTypes = Enum.GetValues<AlertType>().Except(AlertsQueryExtensions.AlertsWithCustomQueries);

            // Act
            var total = await sut.GetTotal(alertTypes);

            // Assert
            Assert.NotEqual(0, total);
        }

        /// <summary>
        /// This method is used to build a new instance of UserContext with the values of a valid user from the targetted DB. Note: The values of the user below only apply to the dev database.
        /// </summary>
        /// <returns>Returns an instance of UserContext with the values of a valid user from the target DB.</returns>
        private static UserContext GetRealUser() => new()
        {
            IsMLSAdministrator = true,
            Id = new Guid("9AD0A512-0D81-4F40-BAAF-1E7A2AFABBD1"),
            Email = "freddy@homesusa.com",
            Name = "Freddy Zambrano",
            UserRole = UserRole.MLSAdministrator,
            CompanyId = new Guid("2cdd4a1e-7c90-4a04-8cb1-2c39853c5a32"),
            EmployeeRole = RoleEmployee.CompanyAdmin,
        };
    }
}
