namespace Husa.Quicklister.Abor.Api.Tests.ShowingTime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Interfaces.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Models.ShowingTime;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class ShowingTimeContactsControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IShowingTimeContactService> contactService = new();
        private readonly Mock<IShowingTimeContactQueriesRepository> contactQueriesRepository = new();
        private readonly Mock<ILogger<ShowingTimeContactsController>> logger = new();
        public ShowingTimeContactsControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new ShowingTimeContactsController(
                this.contactService.Object,
                this.contactQueriesRepository.Object,
                this.logger.Object,
                this.fixture.Mapper);
        }

        public ShowingTimeContactsController Sut { get; private set; }

        [Fact]
        public async Task SearchAsync_Success()
        {
            var data = Array.Empty<ShowingTimeContactQueryResult>();
            this.contactQueriesRepository.Setup(x => x.GetAsync(It.IsAny<ShowingTimeContactQueryFilter>()))
                .ReturnsAsync(new DataSet<ShowingTimeContactQueryResult>(data, data.Length))
                .Verifiable();

            var filtersRequest = new Mock<ShowingTimeContactRequestFilter>().Object;
            await this.Sut.SearchAsync(filtersRequest);

            this.contactQueriesRepository
                .Verify(x => x.GetAsync(It.IsAny<ShowingTimeContactQueryFilter>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            this.contactQueriesRepository
                .Setup(x => x.GetContactById(It.IsAny<Guid>()))
                .ReturnsAsync(new Mock<ShowingTimeContactDetailQueryResult>().Object)
                .Verifiable();

            await this.Sut.GetAsync(Guid.Empty);

            this.contactQueriesRepository
                .Verify(x => x.GetContactById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Success()
        {
            this.contactService
                .Setup(x => x.CreateAsync(It.IsAny<ContactDto>()))
                .ReturnsAsync(Guid.Empty)
                .Verifiable();

            await this.Sut.CreateAsync(new Mock<ContactRequest>().Object);

            this.contactService.Verify(x => x.CreateAsync(It.IsAny<ContactDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            this.contactService
                .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ContactDto>()))
                .ReturnsAsync(true)
                .Verifiable();

            await this.Sut.UpdateAsync(Guid.Empty, new Mock<ContactRequest>().Object);

            this.contactService.Verify(
                x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ContactDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            this.contactService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Verifiable();

            await this.Sut.DeleteAsync(Guid.Empty);

            this.contactService.Verify(
                 x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task AssignContactAsync_Success()
        {
            this.contactService
                .Setup(x => x.AssignTo(It.IsAny<AssignContactDto>()))
                .Verifiable();

            await this.Sut.AssignContactAsync(new Mock<AssignContactRequest>().Object);

            this.contactService.Verify(x => x.AssignTo(It.IsAny<AssignContactDto>()), Times.Once);
        }

        [Fact]
        public async Task RemoveContactAsync_Success()
        {
            this.contactService
                .Setup(x => x.RemoveFrom(It.IsAny<AssignContactDto>()))
                .Verifiable();

            await this.Sut.RemoveContactAsync(new Mock<AssignContactRequest>().Object);

            this.contactService.Verify(x => x.RemoveFrom(It.IsAny<AssignContactDto>()), Times.Once);
        }
    }
}
