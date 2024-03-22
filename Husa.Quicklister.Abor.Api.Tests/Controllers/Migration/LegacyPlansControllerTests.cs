namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Migration
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Controllers.Migration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LegacyPlansControllerTests
    {
        [Fact]
        public async Task MigrateFromV1_ReturnsOk()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCommunityId = 123;
            var fromDate = DateTime.Now;

            var loggerMock = new Mock<ILogger<LegacyPlansController>>();
            var migrationServiceMock = new Mock<IPlanMigrationService>();

            var controller = new LegacyPlansController(migrationServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.MigrateFromV1(companyId, true, true, legacyCommunityId, fromDate);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
