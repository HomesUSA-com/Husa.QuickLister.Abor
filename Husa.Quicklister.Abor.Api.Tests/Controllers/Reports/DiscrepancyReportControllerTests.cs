namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Controllers.Reports;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Reports;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Reports;
    using Husa.Quicklister.Extensions.Application.Interfaces.Reports;
    using Husa.Quicklister.Extensions.Application.Models.Reports;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class DiscrepancyReportControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IDiscrepancyReportService> discrepancyReportService = new();
        private readonly Mock<ILogger<DiscrepancyReportController>> logger = new();
        public DiscrepancyReportControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new DiscrepancyReportController(
                this.discrepancyReportService.Object,
                this.logger.Object,
                this.fixture.Mapper);
        }

        public DiscrepancyReportController Sut { get; set; }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAsync_Success(bool isDetail)
        {
            // Arrange
            var request = new DiscrepancyReportRequest()
            {
                IsDetail = isDetail,
                Skip = 0,
                Top = 50,
            };
            var mlsNumbers = new List<string> { "mlsnumber-1", "mlsnumber-2", "mlsnumber-3", "mlsnumber-4", "mlsnumber-5" };
            var details = GenerateDiscrepancyDetailResult(mlsNumbers);
            var analysis = new DiscrepancyAnalysisResult
            {
                XmlListings = 4,
                MlsListings = 5,
                ListingsInBoth = 4,
                ListingsPriceDiscrepancy = 1,
                TotalDiscrepancy = 10000,
                AverageDiscrepancy = 10000,
            };
            this.discrepancyReportService.Setup(x => x.GetDiscrepancyAnalysisAsync()).ReturnsAsync(analysis);
            this.discrepancyReportService.Setup(x => x.GetDiscrepancyDetail(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(details);

            // Act
            var actionResult = await this.Sut.GetAsync(request);

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            if (isDetail)
            {
                Assert.IsAssignableFrom<DataSet<DiscrepancyDetailResponse>>(okObjectResult.Value);
            }
            else
            {
                Assert.IsAssignableFrom<DiscrepancyAnalysisResponse>(okObjectResult.Value);
            }
        }

        private static DataSet<DiscrepancyDetailResult> GenerateDiscrepancyDetailResult(IEnumerable<string> mlsNumbers)
        {
            var data = new List<DiscrepancyDetailResult>();
            foreach (var id in mlsNumbers)
            {
                var discrepancyDetail = new DiscrepancyDetailResult
                {
                    MlsNumber = id,
                    MlsStatus = "Active",
                    XmlStatus = "XmlStatus",
                    Community = "Community",
                    StreetName = "StreetName",
                    StreetNumber = "StreetNumber",
                    City = "City",
                    Zip = "Zip",
                };
                data.Add(discrepancyDetail);
            }

            return new DataSet<DiscrepancyDetailResult>(data, data.Count);
        }
    }
}
