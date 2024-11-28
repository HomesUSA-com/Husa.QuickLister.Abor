namespace Husa.Quicklister.Abor.Application.Tests.Services.Communities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.JsonImport.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class CommunityJsonImportServiceTests
    {
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<ILogger<CommunityJsonImportService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IJsonImportClient> jsonClient = new();

        public CommunityJsonImportServiceTests()
        {
            var jsonCommunityClientMock = new Mock<IJsonImportCommunity>();
            this.jsonClient.SetupGet(x => x.Community).Returns(jsonCommunityClientMock.Object);

            this.Sut = new CommunityJsonImportService(
                this.jsonClient.Object,
                this.communitySaleRepository.Object,
                this.userContextProvider.Object,
                this.logger.Object);
        }

        public ICommunityJsonImportService Sut { get; set; }

        [Fact]
        public async Task ImportCommunity_CreateNewCommunity_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var jsonCommunityId = Guid.NewGuid();

            this.jsonClient
                .Setup(x => x.Community.GetByIdAsync(It.Is<Guid>(id => id == jsonCommunityId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCommunityResponse(jsonCommunityId))
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, jsonCommunityId);

            // Assert
            this.communitySaleRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
            this.communitySaleRepository.Verify(r => r.Attach(It.IsAny<CommunitySale>()), Times.Once);
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private static CommunityResponse GetCommunityResponse(Guid jsonCommunityId, Guid? qlId = null) => new()
        {
            Id = jsonCommunityId,
            Name = Faker.Company.Name(),
            QuicklisterId = qlId,
            SchoolInformation = new()
            {
                SchoolDistrict = "BartlettISD",
                ElementarySchool = "Alma Brewer Strawn",
            },
            Location = new()
            {
                ZipCode = "45444",
            },
            SalesOfficeLocation = new()
            {
                StreetName = "SO StreetName",
                StreetNum = "SO StreetNum",
            },
            Amenities = new[]
            {
                Amenities.BoatRamp,
                Amenities.ClubHouse,
                Amenities.CommunityPool,
                Amenities.ElectricChargingStation,
                Amenities.FitnessCenter,
                Amenities.Golf,
                Amenities.GuardedEntrance,
                Amenities.JoggingOrBikePath,
                Amenities.Lake,
                Amenities.Park,
                Amenities.Playground,
                Amenities.Sauna,
                Amenities.Spa,
                Amenities.TennisCourts,
            },
        };
    }
}
