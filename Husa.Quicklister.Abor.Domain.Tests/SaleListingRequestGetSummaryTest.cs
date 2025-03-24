namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Tests.Providers;
    using Xunit;

    public class SaleListingRequestGetSummaryTest
    {
        [Fact]
        public void GetSummary_WithNoChanges_ReturnsEmptyArray()
        {
            // Arrange
            var saleListing = TestProviderListing.CreateSaleListing();
            var currentRequest = new SaleListingRequest(saleListing, Guid.NewGuid());
            var previousRequest = currentRequest.Clone();

            // Act
            var result = currentRequest.GetSummary(previousRequest);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetSummary_WithChanges_ReturnsNonEmptySummary()
        {
            // Arrange
            var saleListing = TestProviderListing.CreateSaleListing();
            var currentRequest = new SaleListingRequest(saleListing, Guid.NewGuid());
            var previousRequest = currentRequest.Clone();

            // Make a change to trigger summary generation
            currentRequest.ListPrice = currentRequest.ListPrice + 10000;

            // Act
            var result = currentRequest.GetSummary(previousRequest).ToList();

            // Assert
            Assert.NotEmpty(result);
            var rootSection = result.FirstOrDefault(s => s.Name == SaleListingRequest.SummarySection);
            Assert.NotNull(rootSection);

            var listPriceField = rootSection.Fields.FirstOrDefault(f => f.FieldName == nameof(currentRequest.ListPrice));
            Assert.NotNull(listPriceField);
            Assert.Equal(previousRequest.ListPrice, listPriceField.OldValue);
            Assert.Equal(currentRequest.ListPrice, listPriceField.NewValue);
        }

        [Fact]
        public void GetSummary_AlwaysIncludesMlsStatus_EvenWhenNotChanged()
        {
            // Arrange
            var saleListing = TestProviderListing.CreateSaleListing();
            var currentRequest = new SaleListingRequest(saleListing, Guid.NewGuid());
            var previousRequest = currentRequest.Clone();

            // Make a change to trigger summary generation, but not to MlsStatus
            currentRequest.ListPrice = currentRequest.ListPrice + 10000;

            // Act
            var result = currentRequest.GetSummary(previousRequest).ToList();

            // Assert
            Assert.NotEmpty(result);
            var rootSection = result.FirstOrDefault(s => s.Name == SaleListingRequest.SummarySection);
            Assert.NotNull(rootSection);

            var mlsStatusField = rootSection.Fields.FirstOrDefault(f => f.FieldName == nameof(SaleListingRequest.MlsStatus));
            Assert.NotNull(mlsStatusField);
            Assert.Equal(currentRequest.MlsStatus, mlsStatusField.OldValue);
            Assert.Equal(currentRequest.MlsStatus, mlsStatusField.NewValue);
        }

        [Fact]
        public void GetSummary_WhenTransitioningFromPendingToClosed_IncludesAgentId()
        {
            // Arrange
            var saleListing = TestProviderListing.CreateSaleListing(MarketStatuses.Pending);
            var currentRequest = new SaleListingRequest(saleListing, Guid.NewGuid());
            var previousRequest = currentRequest.Clone();

            // Change status from Pending to Closed
            currentRequest.MlsStatus = MarketStatuses.Closed;
            currentRequest.StatusFieldsInfo.ClosePrice = 154555;

            // Act
            var result = currentRequest.GetSummary(previousRequest).ToList();

            // Assert
            Assert.NotEmpty(result);

            // Find the status fields section
            var statusSection = result.FirstOrDefault(s => s.Name == SaleStatusFieldsRecord.SummarySection);
            Assert.NotNull(statusSection);

            // Verify AgentId is included
            var agentIdField = statusSection.Fields.FirstOrDefault(f => f.FieldName == nameof(currentRequest.StatusFieldsInfo.AgentId));
            Assert.NotNull(agentIdField);
            Assert.Equal(currentRequest.StatusFieldsInfo.AgentId, agentIdField.NewValue);
        }

        [Fact]
        public void GetSummary_WithNullPreviousRequest_ReturnsFullSummary()
        {
            // Arrange
            var saleListing = TestProviderListing.CreateSaleListing();
            var currentRequest = new SaleListingRequest(saleListing, Guid.NewGuid());

            // Act
            var result = currentRequest.GetSummary<SaleListingRequest>(null).ToList();

            // Assert
            Assert.NotEmpty(result);

            // Should include root section
            Assert.Contains(result, s => s.Name == SaleListingRequest.SummarySection);

            // Should include property sections
            Assert.Contains(result, s => s.Name == SalePropertyRecord.SummarySection);

            // Should include status fields section
            Assert.Contains(result, s => s.Name == SaleStatusFieldsRecord.SummarySection);
        }
    }
}
