namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Classes;
    using Husa.MediaService.Client;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Moq;
    using Xunit;
    using CompanyRequest = Husa.CompanyServicesManager.Api.Contracts.Request;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;
    using MediaResponse = Husa.MediaService.Api.Contracts.Response;
    using XmlRequest = Husa.Xml.Api.Contracts.Request;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public class ResidentialIdxQueriesRepositoryTests
    {
        private readonly Mock<IXmlClientWithToken> xmlClient = new();
        private readonly Mock<IServiceSubscriptionClient> companyClient = new();
        private readonly Mock<IMediaServiceClient> mediaClient = new();

        [Fact]
        public async Task FindByBuilderName_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var companyName = "Company Name";
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId, communityId: communityId);
            listing.XmlListingId = Guid.NewGuid();
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            var sut = this.GetInMemoryRepository(new[] { listing }, new[] { community });

            var xmlListing = new XmlResponse.XmlListingResponse()
            {
                Id = listing.XmlListingId.Value,
                Number = "spec-name",
            };
            this.xmlClient
                .Setup(x => x.Listing.GetAsync(It.IsAny<XmlRequest.ListingRequestFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<XmlResponse.XmlListingResponse>([xmlListing], 1));

            var company = new CompanyResponse.Company() { Id = companyId };
            this.companyClient
                .Setup(x => x.Company.GetAsync(It.IsAny<CompanyRequest.CompanyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<CompanyResponse.Company>([company], 1));

            this.mediaClient.Setup(x => x.GetResources(listingId, MediaService.Domain.Enums.MediaType.Residential, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MediaResponse.ResourceResponse()
                {
                    Media = new MediaResponse.MediaDetail[]
                    {
                        new()
                        {
                            Title = "Image1",
                            Order = 1,
                            Uri = new Uri("https://husatest/image/1"),
                        },
                    },
                    VirtualTour = new MediaResponse.VirtualTourDetail[]
                    {
                        new()
                        {
                            Title = "VT1",
                            Uri = new Uri("https://husatest/vt/1"),
                        },
                    },
                });

            // Act
            var result = await sut.FindByBuilderName(companyName);

            // Assert
            Assert.Single(result);
            Assert.Equal(xmlListing.Number, result.Single().SpecNumber);
        }

        private ResidentialIdxQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings, IEnumerable<CommunitySale> communities)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Community.AddRange(communities);
            dbContext.ListingSale.AddRange(listings);
            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new ResidentialIdxQueriesRepository(
                queriesDbContext,
                this.xmlClient.Object,
                this.mediaClient.Object,
                this.companyClient.Object);
        }
    }
}
