namespace Husa.Quicklister.Abor.Api.Tests.Community
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Notes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Notes;
    using Husa.Quicklister.Abor.Api.Controllers.Notes;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class CommunitySaleNotesControllerTests
    {
        private readonly Mock<ICommunityNotesService> communityNotesService = new();
        private readonly Mock<ILogger<SaleCommunityNotesController>> logger = new();
        private readonly ApplicationServicesFixture fixture;

        public CommunitySaleNotesControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleCommunityNotesController(
                this.communityNotesService.Object,
                this.logger.Object,
                this.fixture.Mapper);
        }

        public SaleCommunityNotesController Sut { get; set; }

        [Fact]
        public async Task GetNotes_NotesFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId1 = Guid.NewGuid();
            var noteDetail1 = TestModelProvider.GetNoteDetailResult(noteId1);
            var noteId2 = Guid.NewGuid();
            var noteDetail2 = TestModelProvider.GetNoteDetailResult(noteId2);

            this.communityNotesService.Setup(m => m.GetNotes(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(new[] { noteDetail1, noteDetail2 })
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNotes(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<NotesResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Id == noteId1);
            this.communityNotesService.Verify();
        }

        [Fact]
        public async Task GetNotes_NotesEmpty_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteDetailList = new List<NoteDetailResult>()
            {
            };

            this.communityNotesService.Setup(m => m.GetNotes(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(noteDetailList)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNotes(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<NotesResponse>>(okObjectResult.Value);
            Assert.Empty(result);
            this.communityNotesService.Verify();
        }

        [Fact]
        public async Task CreateNotes_NotesAdded_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            this.communityNotesService.Setup(m => m.CreateNote(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            var note = new NoteRequest
            {
                Id = entityId,
                Title = "Test",
                Description = "Test",
            };

            // Act
            var actionResult = await this.Sut.CreateAsync(entityId, note);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.communityNotesService.Verify(
                x => x.CreateNote(
                    It.Is<Guid>(x => x == entityId),
                    It.Is<string>(title => title.EqualsTo(note.Title)),
                    It.Is<string>(description => description.EqualsTo(note.Title) && description.EqualsTo(note.Description))),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNotes_NotesUpdated_Success()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var note = new NoteRequest
            {
                Id = entityId,
                Title = "Test",
                Description = "Test",
            };

            this.communityNotesService.Setup(m => m.UpdateNote(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.UpdateAsync(entityId, noteId, note);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.communityNotesService.Verify(
                x => x.UpdateNote(
                    It.Is<Guid>(id => id == entityId),
                    It.Is<Guid>(idOfNote => idOfNote == noteId),
                    It.Is<string>(title => title.EqualsTo(note.Title)),
                    It.Is<string>(description => description.EqualsTo(note.Title) && description.EqualsTo(note.Description))),
                Times.Once);
        }

        [Fact]
        public async Task GetNoteById_NoteFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var noteDetail = TestModelProvider.GetNoteDetailResult(noteId);

            this.communityNotesService.Setup(m => m.GetNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)))
                .ReturnsAsync(noteDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNoteById(entityId, noteId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<NotesResponse>(okObjectResult.Value);
            Assert.Equal(result.Id, noteId);
            this.communityNotesService.Verify();
        }

        [Fact]
        public async Task GetNoteById_NoteNotFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            this.communityNotesService.Setup(m => m.GetNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)))
                .ReturnsAsync((NoteDetailResult)null)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNoteById(entityId, noteId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteById_NoteDeleted_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            this.communityNotesService.Setup(x => x.DeleteNote(It.Is<Guid>(x => x == entityId), It.IsAny<Guid>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteById(entityId, noteId));
            this.communityNotesService.Verify(x => x.DeleteNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)), Times.Once);
        }
    }
}
