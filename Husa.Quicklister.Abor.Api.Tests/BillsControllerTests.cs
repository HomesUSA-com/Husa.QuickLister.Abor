namespace Husa.Quicklister.Abor.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Quickbooks.Models.Invoice;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Reports;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class BillsControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<ISaleListingRequestQueriesRepository> saleListingRequestQueriesRepository;
        private readonly Mock<ILogger<BillsController>> logger;
        private readonly Mock<ISaleListingBillService> saleListingBillService;

        public BillsControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.saleListingRequestQueriesRepository = new Mock<ISaleListingRequestQueriesRepository>();
            this.saleListingBillService = new Mock<ISaleListingBillService>();
            this.logger = new Mock<ILogger<BillsController>>();
            this.Sut = new BillsController(
                this.saleListingRequestQueriesRepository.Object,
                this.logger.Object,
                this.saleListingBillService.Object,
                this.fixture.Mapper);
        }

        public BillsController Sut { get; set; }

        [Fact]
        public async Task GetBillingListingAsync_ListingFound_Success()
        {
            // Arrange
            var listingId1 = Guid.NewGuid();
            var listingId2 = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing1 = TestModelProvider.GetListingSaleBillingQueryResult(listingId1);
            var listing2 = TestModelProvider.GetListingSaleBillingQueryResult(listingId2);

            var listingSaleResponse = new List<ListingSaleBillingQueryResult>()
                {
                    listing1,
                    listing2,
                };

            var filter = new ListingSaleBillingRequestFilter
            {
                CompanyId = companyId,
                SearchBy = string.Empty,
                Skip = 0,
                Take = 100,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow,
            };

            var dataSet = new DataSet<ListingSaleBillingQueryResult>(listingSaleResponse, listingSaleResponse.Count);

            this.saleListingRequestQueriesRepository
            .Setup(m => m.GetBillableListingsAsync(It.IsAny<ListingSaleBillingQueryFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.GetBillingListing(filter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<ListingSaleBillingQueryResult>>(okObjectResult.Value);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Total);
            this.saleListingRequestQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetBillingListingAsync_ListingNotFound_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            var listingSaleResponse = new List<ListingSaleBillingQueryResult>() { };

            var filter = new ListingSaleBillingRequestFilter
            {
                CompanyId = companyId,
                SearchBy = string.Empty,
                Skip = 0,
                Take = 100,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow,
            };

            var dataSet = new DataSet<ListingSaleBillingQueryResult>(listingSaleResponse, listingSaleResponse.Count);

            this.saleListingRequestQueriesRepository
            .Setup(m => m.GetBillableListingsAsync(It.IsAny<ListingSaleBillingQueryFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.GetBillingListing(filter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<ListingSaleBillingQueryResult>>(okObjectResult.Value);
            Assert.Empty(result.Data);
            this.saleListingRequestQueriesRepository.Verify();
        }

        [Fact]
        public async Task CreateInvoiceAsyncSuccess()
        {
            // Arrange
            var invoiceId = "234";
            var companyId = Guid.NewGuid();
            var listingIds = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };
            var invoiceRequest = new InvoiceRequest()
            {
                CompanyId = companyId,
                ListingIds = listingIds,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
            };
            var invoiceResult = CommandSingleResult<string, string>.Success(invoiceId);
            this.saleListingBillService
            .Setup(m => m.ProcessInvoice(It.IsAny<InvoiceDto>()))
            .ReturnsAsync(invoiceResult)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.CreateInvoice(invoiceRequest);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.NotNull(okObjectResult.Value);
            Assert.Equal(invoiceId, okObjectResult.Value);
            this.saleListingRequestQueriesRepository.Verify();
        }
    }
}
