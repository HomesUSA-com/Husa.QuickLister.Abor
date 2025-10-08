namespace Husa.Quicklister.Abor.Application.Tests.Services.ShowingTime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.ShowingTime.Enums;
    using Husa.Quicklister.Abor.Application.Services.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class ShowingTimeContactServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly ShowingTimeContactService contactService;
        private readonly Mock<IUserContextProvider> userContextProvider;
        private readonly Mock<IShowingTimeContactRepository> contactRepository;

        public ShowingTimeContactServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.userContextProvider
                .Setup(x => x.GetCurrentUser())
                .Returns(new Mock<IUserContext>().Object);
            this.contactRepository = new Mock<IShowingTimeContactRepository>();
            this.contactService = new ShowingTimeContactService(
                this.fixture.Mapper, this.userContextProvider.Object, this.contactRepository.Object);
        }

        [Fact]
        public async Task ContactCreate_Success()
        {
            var contact = this.ContactFactory(Guid.Empty, ContactScope.Community);
            this.contactRepository
                .Setup(c => c.Attach(It.IsAny<ShowingTimeContact>()))
                .Verifiable();

            await this.contactService.CreateAsync(contact);

            this.contactRepository.Verify();
        }

        [Fact]
        public async Task ContactUpdate_Success()
        {
            var contactId = Guid.NewGuid();
            var dto = this.ContactFactory(contactId, ContactScope.Community);
            var contact = this.fixture.Mapper.Map<ShowingTimeContact>(dto);

            this.contactRepository
                .Setup(x => x.GetById(contactId, It.IsAny<bool>()))
                .ReturnsAsync(contact)
                .Verifiable();
            this.contactRepository
                .Setup(x => x.UpdateAsync(It.IsAny<ShowingTimeContact>()))
                .Verifiable();

            await this.contactService.UpdateAsync(contactId, dto);

            this.contactRepository.Verify();
        }

        [Fact]
        public async Task ContactDelete_Success()
        {
            var contact = this.fixture.Mapper.Map<ShowingTimeContact>(
                this.ContactFactory(Guid.Empty, ContactScope.Community));
            this.contactRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(contact);
            this.contactRepository
                .Setup(x => x.UpdateAsync(It.IsAny<ShowingTimeContact>()))
                .Verifiable();
            this.contactRepository
                .Setup(x => x.RemoveFromCommunity(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Verifiable();
            this.contactRepository
                .Setup(x => x.RemoveFromListing(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Verifiable();

            await this.contactService.DeleteAsync(contact.Id);

            this.contactRepository.Verify();
        }

        [Fact]
        public async Task ContactAssignToCommunity_Success()
        {
            var scope = ContactScope.Community;
            var dto = new AssignContactDto
            {
                ContactId = Guid.NewGuid(),
                Scope = scope,
                ScopeId = Guid.NewGuid(),
            };

            this.contactRepository.Setup(x => x.AssignToCommunity(
                dto.ContactId, dto.ScopeId.Value, It.IsAny<int?>()))
                .Verifiable();

            await this.contactService.AssignTo(dto);

            this.contactRepository.Verify();
        }

        [Fact]
        public async Task ContactRemoveFromCommunity_Success()
        {
            var dto = new AssignContactDto
            {
                ContactId = Guid.NewGuid(),
                Scope = ContactScope.Community,
                ScopeId = Guid.NewGuid(),
            };

            this.contactRepository
                .Setup(x => x.RemoveFromCommunity(dto.ContactId, dto.ScopeId.Value))
                .Verifiable();

            await this.contactService.RemoveFrom(dto);

            this.contactRepository.Verify();
        }

        private ContactDto ContactFactory(Guid id, ContactScope scope) => new ContactDto
        {
            Id = id,
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Email = Faker.Internet.Email(),
            MobilePhone = Faker.Phone.Number(),
            OfficePhone = Faker.Phone.Number(),
            Scope = scope,
        };
    }
}
