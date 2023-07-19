namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]

    public class QueryMediaRepositoryTest
    {
        private readonly Mock<IMediaServiceClient> mediaServiceClien = new();

        public QueryMediaRepositoryTest()
        {
            this.Sut = new QueryMediaRepository(this.mediaServiceClien.Object);
        }

        public IQueryMediaRepository Sut { get; set; }

        [Fact]
        public async Task GetMediaSummary_Success()
        {
            // Arrange
            var currentRequestId = Guid.NewGuid();
            var lastestCompletedRequestId = Guid.NewGuid();
            var fileDetail = TestModelProvider.GetMediaDetail();
            var virtualTour = TestModelProvider.GetVirtualTourDetail();

            // lastest Completed Request media
            var lastestFiles = new List<MediaDetail>()
            {
                fileDetail,
                TestModelProvider.GetMediaDetail(),
            };
            var lastestVirtualTours = new List<VirtualTourDetail>()
            {
                virtualTour,
                TestModelProvider.GetVirtualTourDetail(),
            };
            var lastestResourceResponse = new ResourceResponse { Media = lastestFiles, VirtualTour = lastestVirtualTours };
            this.mediaServiceClien.Setup(u => u.GetResources(It.Is<Guid>(id => id == lastestCompletedRequestId), It.Is<MediaType>(type => type == MediaType.ListingRequest), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lastestResourceResponse)
                .Verifiable();

            // current Request media
            var currentFiles = new List<MediaDetail>()
            {
                TestModelProvider.GetMediaDetail(fileDetail.Id),
                TestModelProvider.GetMediaDetail(),
            };
            var currentVirtualTours = new List<VirtualTourDetail>()
            {
                virtualTour,
                TestModelProvider.GetVirtualTourDetail(),
            };
            var currentResourceResponse = new ResourceResponse { Media = currentFiles, VirtualTour = currentVirtualTours };
            this.mediaServiceClien.Setup(u => u.GetResources(It.Is<Guid>(id => id == currentRequestId), It.Is<MediaType>(type => type == MediaType.ListingRequest), It.IsAny<CancellationToken>()))
                .ReturnsAsync(currentResourceResponse)
                .Verifiable();

            // Act
            var summary = await this.Sut.GetMediaSummary(currentRequestId, lastestCompletedRequestId);
            var filesSummary = summary.FirstOrDefault(x => x.Name == "Media");
            var virtualToursSummary = summary.FirstOrDefault(x => x.Name == "MediaVirtualTour");

            // Assert
            Assert.Equal(3, filesSummary.Fields.Count());
            Assert.Single(filesSummary.Fields.Where(x => x.NewValue == null));
            Assert.Single(filesSummary.Fields.Where(x => x.OldValue == null));
            Assert.Single(filesSummary.Fields.Where(x => x.NewValue != null && x.OldValue != null));
            Assert.Equal(2, virtualToursSummary.Fields.Count());
            Assert.Single(virtualToursSummary.Fields.Where(x => x.NewValue == null));
            Assert.Single(virtualToursSummary.Fields.Where(x => x.OldValue == null));
        }

        [Fact]
        public async Task GetMediaSummary_WithoutLastRequest_Success()
        {
            // Arrange
            var currentRequestId = Guid.NewGuid();
            var currentFiles = new List<MediaDetail>()
            {
                TestModelProvider.GetMediaDetail(),
                TestModelProvider.GetMediaDetail(),
            };
            var currentVirtualTours = new List<VirtualTourDetail>()
            {
                TestModelProvider.GetVirtualTourDetail(),
            };
            var currentResourceResponse = new ResourceResponse { Media = currentFiles, VirtualTour = currentVirtualTours };
            this.mediaServiceClien.Setup(u => u.GetResources(It.Is<Guid>(id => id == currentRequestId), It.Is<MediaType>(type => type == MediaType.ListingRequest), It.IsAny<CancellationToken>()))
                .ReturnsAsync(currentResourceResponse)
                .Verifiable();
            this.mediaServiceClien.Setup(u => u.GetResources(It.Is<Guid>(id => id != currentRequestId), It.Is<MediaType>(type => type == MediaType.ListingRequest), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResourceResponse)null)
                .Verifiable();

            // Act
            var summary = await this.Sut.GetMediaSummary(currentRequestId, null);
            var filesSummary = summary.FirstOrDefault(x => x.Name == "Media");
            var virtualToursSummary = summary.FirstOrDefault(x => x.Name == "MediaVirtualTour");

            // Assert
            Assert.Equal(currentFiles.Count, filesSummary.Fields.Count(x => x.OldValue == null));
            Assert.Equal(currentVirtualTours.Count, virtualToursSummary.Fields.Count(x => x.OldValue == null));
            Assert.Empty(filesSummary.Fields.Where(x => x.NewValue == null));
            Assert.Empty(virtualToursSummary.Fields.Where(x => x.NewValue == null));
        }
    }
}
