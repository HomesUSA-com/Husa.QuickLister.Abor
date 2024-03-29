namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Migration
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Controllers.Migration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LegacyCommunitiesControllerTests
    {
        [Fact]
        public async Task MigrateFromV1_ReturnsOk()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCommunityId = 123;
            var fromDate = DateTime.Now;

            var loggerMock = new Mock<ILogger<LegacyCommunitiesController>>();
            var communityMigrationServiceMock = new Mock<ICommunityMigrationService>();

            var controller = new LegacyCommunitiesController(communityMigrationServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.MigrateFromV1(companyId, true, true, legacyCommunityId, fromDate);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
