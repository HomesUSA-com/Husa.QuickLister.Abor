namespace Husa.Quicklister.Abor.Api.Tests.Listing
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
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class ListingSaleNotesControllerTests
    {
        private readonly Mock<ISaleListingService> listingSaleService = new();
        private readonly Mock<ISaleListingNotesService> listingNotesService = new();
        private readonly Mock<ILogger<SaleListingNotesController>> logger = new();
        private readonly ApplicationServicesFixture fixture;

        public ListingSaleNotesControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleListingNotesController(
                this.listingNotesService.Object,
                this.logger.Object,
                this.fixture.Mapper);
        }

        public SaleListingNotesController Sut { get; set; }

        [Fact]
        public async Task GetNotes_NotesFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId1 = Guid.NewGuid();
            var noteDetail1 = TestModelProvider.GetNoteDetailResult(noteId1);
            var noteId2 = Guid.NewGuid();
            var noteDetail2 = TestModelProvider.GetNoteDetailResult(noteId2);

            this.listingNotesService.Setup(m => m.GetNotes(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(new[] { noteDetail1, noteDetail2 })
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNotes(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<NotesResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Id == noteId1);
            this.listingSaleService.Verify();
        }

        [Fact]
        public async Task GetNotes_NotesEmpty_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            this.listingNotesService.Setup(m => m.GetNotes(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(Array.Empty<NoteDetailResult>())
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNotes(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<NotesResponse>>(okObjectResult.Value);
            Assert.Empty(result);
            this.listingSaleService.Verify();
        }

        [Fact]
        public async Task CreateNotes_NotesAdded_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var simpleNote = new NoteRequest
            {
                Id = entityId,
                Title = "Test",
                Description = "Test",
            };

            this.listingNotesService.Setup(m => m.CreateNote(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.CreateAsync(entityId, simpleNote);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.listingNotesService.Verify(
                x => x.CreateNote(
                    It.Is<Guid>(id => id == entityId),
                    It.Is<string>(title => title.EqualsTo(simpleNote.Title)),
                    It.Is<string>(description => description.EqualsTo(simpleNote.Description))),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNotes_NotesUpdated_Success()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var simpleNote = new NoteRequest
            {
                Id = entityId,
                Title = "Test",
                Description = "Test",
            };
            this.listingNotesService.Setup(m => m.UpdateNote(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.UpdateAsync(entityId, noteId, simpleNote);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.listingNotesService.Verify(
                x => x.UpdateNote(
                    It.Is<Guid>(id => id == entityId),
                    It.Is<Guid>(idOfNote => idOfNote == noteId),
                    It.Is<string>(title => title.EqualsTo(simpleNote.Title)),
                    It.Is<string>(description => description.EqualsTo(simpleNote.Description))),
                Times.Once);
        }

        [Fact]
        public async Task GetNoteById_NoteFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var noteDetail = TestModelProvider.GetNoteDetailResult(noteId);

            this.listingNotesService.Setup(m => m.GetNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)))
                .ReturnsAsync(noteDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetNoteById(entityId, noteId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<NotesResponse>(okObjectResult.Value);
            Assert.Equal(result.Id, noteId);
            this.listingSaleService.Verify();
        }

        [Fact]
        public async Task GetNoteById_NoteNotFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            this.listingNotesService.Setup(m => m.GetNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)))
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

            this.listingNotesService.Setup(x => x.DeleteNote(It.Is<Guid>(x => x == entityId), It.IsAny<Guid>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteNote(entityId, noteId));
            this.listingNotesService.Verify(x => x.DeleteNote(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == noteId)), Times.Once);
        }
    }
}
