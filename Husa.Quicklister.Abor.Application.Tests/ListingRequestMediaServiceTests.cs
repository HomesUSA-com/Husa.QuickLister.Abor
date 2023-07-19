namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Services.ListingRequests;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.MediaService.Api.Contracts.Request;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class ListingRequestMediaServiceTests
    {
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ISaleListingRequestRepository> saleRequestRepository = new();
        private readonly Mock<IMediaServiceClient> mediaServiceClient = new();
        private readonly Mock<ServiceBusClient> serviceBusClient = new();
        private readonly Mock<IProvideTraceId> traceIdProvider = new();
        private readonly Mock<ILogger<ListingRequestMediaService>> logger = new();
        private readonly ApplicationServicesFixture fixture;

        public ListingRequestMediaServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task GetResourcesThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);
            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.GetResources(listingRequestId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task GetResourcesThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);
            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.GetResources(listingRequestId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task GetResourcesSuccess()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            salePropertyRecord
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId, companyId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mediaDetail = new MediaDetail
            {
                Id = mediaId,
                IsPrimary = true,
                MimeType = MimeType.Image,
                Title = "some title",
            };
            var resourceResponse = new ResourceResponse
            {
                VirtualTour = Array.Empty<VirtualTourDetail>(),
                Media = new[] { mediaDetail },
            };

            this.mediaServiceClient
                .Setup(
                    r => r.GetResources(
                        It.Is<Guid>(id => id == listingRequestId),
                        It.Is<MediaType>(t => t == MediaType.ListingRequest),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(resourceResponse);

            var sut = this.GetSut();

            // Act
            var result = await sut.GetResources(listingRequestId);

            // Assert
            var mediaResult = Assert.Single(result.Media);
            Assert.Equal(mediaId, mediaResult.Id);
            this.userContextProvider.Verify();
            this.saleRequestRepository.Verify();
        }

        [Fact]
        public async Task GetByIdThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);
            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.GetById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task GetByIdThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);
            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.GetById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task GetByIdSuccess()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            salePropertyRecord
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId, companyId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mediaDetailResponse = new MediaDetail
            {
                Id = mediaId,
                IsPrimary = true,
                MimeType = MimeType.Image,
                Title = "some title",
            };

            this.mediaServiceClient
                .Setup(
                    r => r.GetMediaById(
                        It.Is<Guid>(id => id == listingRequestId),
                        It.Is<Guid>(t => t == mediaId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediaDetailResponse);

            var sut = this.GetSut();

            // Act
            var mediaResult = await sut.GetById(entityId: listingRequestId, mediaId);

            // Assert
            Assert.Equal(mediaId, mediaResult.Id);
            this.userContextProvider.Verify();
            this.saleRequestRepository.Verify();
        }

        [Fact]
        public async Task CreateThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.CreateAsync(entityId: listingRequestId, simpleMedia: media));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task CreateThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.CreateAsync(entityId: listingRequestId, simpleMedia: media));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task ReplaceThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.ReplaceAsync(entityId: listingRequestId, simpleMedia: media));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task ReplaceThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.ReplaceAsync(entityId: listingRequestId, simpleMedia: media));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task CreateVirtualTourThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var media = new Request.VirtualTour
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.CreateVirtualTourAsync(entityId: listingRequestId, virtualTour: media));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task CreateVirtualTourThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var media = new Request.VirtualTour
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.CreateVirtualTourAsync(entityId: listingRequestId, virtualTour: media));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task DeleteByIdThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.DeleteById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task DeleteByIdThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.DeleteById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task DeleteVirtualTourByIdThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.DeleteVirtualTourById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task DeleteVirtualTourByIdThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.DeleteVirtualTourById(entityId: listingRequestId, mediaId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task DeleteResourcesThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.DeleteResources(entityId: listingRequestId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task DeleteResourcesThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.DeleteResources(entityId: listingRequestId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task UpdateThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.UpdateAsync(entityId: listingRequestId, mediaId, simpleMedia: media));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task UpdateThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.UpdateResourcesAsync(entityId: listingRequestId, simpleMedia: new[] { media }));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task UpdateResourcesThrowsNotFoundException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.UpdateResourcesAsync(entityId: listingRequestId, simpleMedia: new[] { media }));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task UpdateResourcesThrowsDomainException()
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var media = new Request.SimpleMedia
            {
                EntityId = listingRequestId,
                Id = mediaId,
                Uri = new Uri("http://www.homesusa.com/"),
            };

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.UpdateAsync(entityId: listingRequestId, mediaId, simpleMedia: media));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        [Fact]
        public async Task CreateMediaRequestThrowsNotFoundException()
        {
            // Arrange
            var listingSaleId = Guid.NewGuid();
            var listingRequestId = Guid.NewGuid();
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleListingRequest)null);

            var sut = this.GetSut();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListingRequest>>(() => sut.CreateMediaRequestAsync(listingSaleId: listingSaleId, listingRequestId: listingRequestId));

            // Assert
            Assert.Equal(listingRequestId, notFoundException.Id);
        }

        [Fact]
        public async Task CreateMediaRequestThrowsDomainException()
        {
            // Arrange
            var listingSaleId = Guid.NewGuid();
            var listingRequestId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var saleListing = new Mock<SaleListingRequest>();
            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListing
                .SetupGet(sl => sl.CompanyId)
                .Returns(companyId);

            saleListing
                .SetupGet(sl => sl.SaleProperty)
                .Returns(salePropertyRecord.Object);

            this.SetupValidEntityAndUser(userId);
            this.saleRequestRepository
                .Setup(r => r.GetListingRequestByIdAsync(It.Is<Guid>(id => id == listingRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleListing.Object);

            var sut = this.GetSut();

            // Act
            var domainException = await Assert.ThrowsAsync<DomainException>(() => sut.CreateMediaRequestAsync(listingSaleId: listingSaleId, listingRequestId: listingRequestId));

            // Assert
            Assert.Contains(listingRequestId.ToString(), domainException.Message);
            this.userContextProvider.Verify();
        }

        private void SetupValidEntityAndUser(Guid userId, Guid? companyId = null)
        {
            var user = TestModelProvider.GetCurrentUser(userId, companyId ?? Guid.NewGuid());
            this.userContextProvider
                .Setup(u => u.GetCurrentUser())
                .Returns(user)
                .Verifiable();
        }

        private ListingRequestMediaService GetSut() => new(
            this.fixture.BusOptions.Object,
            this.userContextProvider.Object,
            this.saleRequestRepository.Object,
            this.mediaServiceClient.Object,
            this.serviceBusClient.Object,
            this.traceIdProvider.Object,
            this.logger.Object);
    }
}
