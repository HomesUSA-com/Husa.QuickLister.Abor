namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Notes.Api.Contracts.Request;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using NoteResponses = Husa.Notes.Api.Contracts.Response;
    using RemoteNoteType = Husa.Notes.Domain.Enums.NoteType;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingNotesServiceTests
    {
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ILogger<SaleListingNotesService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<INotesBusService> notesBusService = new();
        private readonly Mock<IUserRepository> userRepository = new();
        private readonly Mock<INotesClient> notesClient = new();

        [Fact]
        public async Task GetListingNotesSuccess()
        {
            // Arrange
            var userRole = UserRole.MLSAdministrator;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();

            var property = new Mock<SaleProperty>();
            listing.Setup(l => l.SaleProperty).Returns(property.Object);
            property.Setup(l => l.CompanyId).Returns(userCompanyId);

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();

            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();
            var noteResponse = new NoteResponses.Note { Id = noteId };

            this.notesClient
                .Setup(ns => ns.GetNotes(It.Is<NoteFilter>(filter => filter.EntityId == listingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { noteResponse })
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var result = await sut.GetNotes(listingId);

            // Assert
            Assert.NotEmpty(result);
            this.listingSaleRepository.Verify();
            this.userContextProvider.Verify();
            this.notesBusService.Verify();
        }

        [Fact]
        public async Task GetListingNoteThrowsNotFoundException()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => sut.GetNote(listingId, noteId));

            // Assert
            Assert.Equal(listingId, noteException.Id);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetListingNoteWhenIsUserWithDifferentCompanyThrowsNotFoundException()
        {
            // Arrange
            var userRole = UserRole.User;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();

            var property = new Mock<SaleProperty>();
            listing.Setup(l => l.SaleProperty).Returns(property.Object);
            property.Setup(l => l.CompanyId).Returns(Guid.NewGuid());

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();
            userMock.Setup(s => s.CompanyId).Returns(userCompanyId).Verifiable();
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => sut.GetNote(listingId, noteId));

            // Assert
            Assert.Equal(listingId, noteException.Id);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetListingNoteSuccess()
        {
            // Arrange
            const string title = "fake title";
            const string description = "fake description";
            const string userName = "fake-user-name";

            var userRole = UserRole.MLSAdministrator;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();

            var property = new Mock<SaleProperty>();
            listing.Setup(l => l.SaleProperty).Returns(property.Object);
            property.Setup(l => l.CompanyId).Returns(userCompanyId);

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();

            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();
            var noteDetail = new NoteResponses.NoteDetail
            {
                Id = noteId,
                Description = description,
                Title = title,
                Type = RemoteNoteType.CommunityProfile,
                MarketCode = MarketCode.SanAntonio,
            };

            this.notesClient
                .Setup(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(noteDetail);

            this.userRepository
                .Setup(u => u.FillUserNameAsync(It.Is<NoteDetailResult>(d => d.Id == noteId)))
                .Callback<IProvideQuicklisterUserInfo>(userInfo =>
                {
                    userInfo.CreatedBy = userName;
                    userInfo.ModifiedBy = userName;
                });

            var sut = this.GetSut();

            // Act
            var noteDetailResult = await sut.GetNote(listingId, noteId);

            // Assert
            Assert.NotNull(noteDetailResult);
            Assert.Equal(title, noteDetailResult.Title);
            Assert.Equal(description, noteDetailResult.Description);
            Assert.Equal(userName, noteDetailResult.CreatedBy);
            Assert.Equal(userName, noteDetailResult.ModifiedBy);
            this.listingSaleRepository.Verify();
            this.userContextProvider.Verify();
            this.userRepository.Verify(u => u.FillUserNameAsync(It.Is<NoteDetailResult>(d => d.Id == noteId)), Times.Once);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateListingNoteThrowsNotFoundException()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var listingId = Guid.NewGuid();
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => sut.CreateNote(listingId, title, description: description));

            // Assert
            Assert.Equal(listingId, noteException.Id);
            this.notesBusService.Verify(
                ns => ns.CreateAsync(
                    It.Is<Guid>(entityId => entityId == listingId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.Residential)),
                Times.Never);
        }

        [Fact]
        public async Task CreateListingNoteSuccess()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var userRole = UserRole.MLSAdministrator;
            var listingId = Guid.NewGuid();
            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);

            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.CreateNote(listingId, title, description: description);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(
                ns => ns.CreateAsync(
                    It.Is<Guid>(entityId => entityId == listingId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.Residential)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateListingNoteThrowsNotFoundException()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var listingId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => sut.UpdateNote(listingId, noteId, title, description));

            // Assert
            Assert.Equal(listingId, noteException.Id);
            this.notesBusService.Verify(
                ns => ns.UpdateAsync(
                    It.Is<Guid>(entityId => entityId == listingId),
                    It.Is<Guid>(id => id == noteId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.Residential)),
                Times.Never);
        }

        [Fact]
        public async Task UpdateListingNoteSuccess()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var userRole = UserRole.MLSAdministrator;
            var listingId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);

            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.UpdateNote(listingId, noteId, title, description);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(
                ns => ns.UpdateAsync(
                    It.Is<Guid>(entityId => entityId == listingId),
                    It.Is<Guid>(id => id == noteId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.Residential)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteListingNoteThrowsNotFoundException()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => sut.DeleteNote(listingId, noteId));

            // Assert
            Assert.Equal(listingId, noteException.Id);
            this.notesBusService.Verify(ns => ns.DeleteByIdAsync(It.Is<Guid>(id => id == noteId)), Times.Never);
        }

        [Fact]
        public async Task DeleteListingNoteSuccess()
        {
            // Arrange
            var userRole = UserRole.MLSAdministrator;
            var listingId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var listing = new Mock<SaleListing>();
            listing.Setup(l => l.Id).Returns(listingId);

            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.DeleteNote(listingId, noteId);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(ns => ns.DeleteByIdAsync(It.Is<Guid>(id => id == noteId)), Times.Once);
        }

        private SaleListingNotesService GetSut() => new(
            this.listingSaleRepository.Object,
            this.userContextProvider.Object,
            this.notesBusService.Object,
            this.userRepository.Object,
            this.notesClient.Object,
            this.logger.Object);
    }
}
