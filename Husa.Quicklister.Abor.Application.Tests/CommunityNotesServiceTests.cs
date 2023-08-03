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
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
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
    public class CommunityNotesServiceTests
    {
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();
        private readonly Mock<ILogger<CommunityNotesService>> logger = new();
        private readonly Mock<INotesBusService> notesBusService = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IUserRepository> userRepository = new();
        private readonly Mock<INotesClient> notesClient = new();

        [Fact]
        public async Task GetCommunityNotesSuccess()
        {
            // Arrange
            var userRole = UserRole.MLSAdministrator;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var communityId = Guid.NewGuid();

            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);
            community.Setup(l => l.CompanyId).Returns(userCompanyId);
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();

            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();
            var noteResponse = new NoteResponses.Note { Id = noteId };

            this.notesClient
                .Setup(ns => ns.GetNotes(It.Is<NoteFilter>(filter => filter.EntityId == communityId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { noteResponse })
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var result = await sut.GetNotes(communityId);

            // Assert
            Assert.NotEmpty(result);
            this.communityRepository.Verify();
            this.userContextProvider.Verify();
            this.notesBusService.Verify();
        }

        [Fact]
        public async Task GetPlanNoteThrowsNotFoundException()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((CommunitySale)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.GetNote(communityId, noteId));

            // Assert
            Assert.Equal(communityId, noteException.Id);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetPlanNoteWhenIsUserWithDifferentCompanyThrowsNotFoundException()
        {
            // Arrange
            var userRole = UserRole.User;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var communityId = Guid.NewGuid();

            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);
            community.Setup(l => l.CompanyId).Returns(Guid.NewGuid());

            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();
            userMock.Setup(s => s.CompanyId).Returns(userCompanyId).Verifiable();
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.GetNote(communityId, noteId));

            // Assert
            Assert.Equal(communityId, noteException.Id);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetCommunityNotesuccess()
        {
            // Arrange
            const string title = "fake title";
            const string description = "fake description";
            const string userName = "fake-user-name";

            var userRole = UserRole.MLSAdministrator;
            var userCompanyId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var communityId = Guid.NewGuid();

            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);
            community.Setup(l => l.CompanyId).Returns(userCompanyId);
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();

            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole).Verifiable();

            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();
            var noteDetail = new NoteResponses.NoteDetail
            {
                Id = noteId,
                Description = description,
                Title = title,
                Type = RemoteNoteType.CommunityProfile,
                MarketCode = MarketCode.Austin,
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
            var noteDetailResult = await sut.GetNote(communityId, noteId);

            // Assert
            Assert.NotNull(noteDetailResult);
            Assert.Equal(title, noteDetailResult.Title);
            Assert.Equal(description, noteDetailResult.Description);
            Assert.Equal(userName, noteDetailResult.CreatedBy);
            Assert.Equal(userName, noteDetailResult.ModifiedBy);
            this.communityRepository.Verify();
            this.userContextProvider.Verify();
            this.userRepository.Verify(u => u.FillUserNameAsync(It.Is<NoteDetailResult>(d => d.Id == noteId)), Times.Once);
            this.notesClient.Verify(ns => ns.GetNoteById(It.Is<Guid>(id => id == noteId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePlanNoteThrowsNotFoundException()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var communityId = Guid.NewGuid();
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((CommunitySale)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.CreateNote(communityId, title, description: description));

            // Assert
            Assert.Equal(communityId, noteException.Id);
            this.notesBusService.Verify(
                ns => ns.CreateAsync(
                    It.Is<Guid>(entityId => entityId == communityId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.CommunityProfile)),
                Times.Never);
        }

        [Fact]
        public async Task CreateCommunityNotesuccess()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var userRole = UserRole.MLSAdministrator;
            var communityId = Guid.NewGuid();
            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);

            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.CreateNote(communityId, title, description: description);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(
                ns => ns.CreateAsync(
                    It.Is<Guid>(entityId => entityId == communityId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.CommunityProfile)),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePlanNoteThrowsNotFoundException()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var communityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((CommunitySale)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.UpdateNote(communityId, noteId, title, description));

            // Assert
            Assert.Equal(communityId, noteException.Id);
            this.notesBusService.Verify(
                ns => ns.UpdateAsync(
                    It.Is<Guid>(entityId => entityId == communityId),
                    It.Is<Guid>(id => id == noteId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.CommunityProfile)),
                Times.Never);
        }

        [Fact]
        public async Task UpdateCommunityNotesuccess()
        {
            // Arrange
            const string title = "this is the title";
            const string description = "this is the description";
            var userRole = UserRole.MLSAdministrator;
            var communityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);

            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.UpdateNote(communityId, noteId, title, description);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(
                ns => ns.UpdateAsync(
                    It.Is<Guid>(entityId => entityId == communityId),
                    It.Is<Guid>(id => id == noteId),
                    It.Is<string>(noteTitle => noteTitle == title),
                    It.Is<string>(noteDescription => noteDescription == description),
                    It.Is<NoteType>(noteType => noteType == NoteType.CommunityProfile)),
                Times.Once);
        }

        [Fact]
        public async Task DeletePlanNoteThrowsNotFoundException()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((CommunitySale)null)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var noteException = await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => sut.DeleteNote(communityId, noteId));

            // Assert
            Assert.Equal(communityId, noteException.Id);
            this.notesBusService.Verify(ns => ns.DeleteByIdAsync(It.Is<Guid>(id => id == noteId)), Times.Never);
        }

        [Fact]
        public async Task DeleteCommunityNotesuccess()
        {
            // Arrange
            var userRole = UserRole.MLSAdministrator;
            var communityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var community = new Mock<CommunitySale>();
            community.Setup(l => l.Id).Returns(communityId);

            this.communityRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();
            var userMock = new Mock<IUserContext>();
            userMock.Setup(s => s.UserRole).Returns(userRole);
            this.userContextProvider.Setup(r => r.GetCurrentUser()).Returns(userMock.Object).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.DeleteNote(communityId, noteId);

            // Assert
            this.userContextProvider.Verify();
            this.notesBusService.Verify(ns => ns.DeleteByIdAsync(It.Is<Guid>(id => id == noteId)), Times.Once);
        }

        private CommunityNotesService GetSut() => new(
                this.communityRepository.Object,
                this.userContextProvider.Object,
                this.notesBusService.Object,
                this.userRepository.Object,
                this.notesClient.Object,
                this.logger.Object);
    }
}
