namespace Husa.Quicklister.Abor.Domain.Tests.SaleListingRequests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class StatusFieldsRecordTests
    {
        [Fact]
        public void UpdateInformation_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new ListingSaleStatusFieldsInfo()
            {
                PendingDate = DateTime.Now,
                EstimatedClosedDate = DateTime.Now,
            };

            // Act
            statusRecord.UpdateInformation(statusInfo);

            // Assert
            Assert.Equal(statusInfo.PendingDate, statusRecord.PendingDate);
            Assert.Equal(statusInfo.EstimatedClosedDate, statusRecord.EstimatedClosedDate);
        }

        [Fact]
        public void GetSummary_ActiveUnderContractFields_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new StatusFieldsRecord()
            {
                PendingDate = DateTime.Now,
                EstimatedClosedDate = DateTime.Now,
                SellConcess = "1223",
            };

            // Act
            var summary = statusRecord.GetSummary(statusInfo, MarketStatuses.ActiveUnderContract);

            // Assert
            Assert.Equal(2, summary.Fields.Count());
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.PendingDate));
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.EstimatedClosedDate));
        }

        [Fact]
        public void GetSummary_ClosedFields_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new StatusFieldsRecord()
            {
                PendingDate = DateTime.Now,
                SellConcess = "1223",
            };

            // Act
            var summary = statusRecord.GetSummary(statusInfo, MarketStatuses.Closed);

            // Assert
            Assert.Equal(2, summary.Fields.Count());
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.PendingDate));
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.SellConcess));
        }

        [Fact]
        public void GetSummary_PendingFields_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new StatusFieldsRecord()
            {
                EstimatedClosedDate = DateTime.Now.AddDays(1),
                PendingDate = DateTime.Now,
                SellConcess = "1223",
            };

            // Act
            var summary = statusRecord.GetSummary(statusInfo, MarketStatuses.Pending);

            // Assert
            Assert.Equal(2, summary.Fields.Count());
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.PendingDate));
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.EstimatedClosedDate));
        }

        [Fact]
        public void GetSummary_HoldFields_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new StatusFieldsRecord()
            {
                BackOnMarketDate = DateTime.Now,
                OffMarketDate = DateTime.Now,
                SellConcess = "1223",
            };

            // Act
            var summary = statusRecord.GetSummary(statusInfo, MarketStatuses.Hold);

            // Assert
            Assert.Equal(2, summary.Fields.Count());
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.BackOnMarketDate));
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.OffMarketDate));
        }

        [Fact]
        public void GetSummary_CanceledFields_Success()
        {
            // Arrange
            var statusRecord = new StatusFieldsRecord();
            var statusInfo = new StatusFieldsRecord()
            {
                BackOnMarketDate = DateTime.Now,
                CancelledReason = "Canceled",
            };

            // Act
            var summary = statusRecord.GetSummary(statusInfo, MarketStatuses.Canceled);

            // Assert
            Assert.Single(summary.Fields);
            Assert.Contains(summary.Fields, x => x.FieldName == nameof(statusInfo.CancelledReason));
        }
    }
}
