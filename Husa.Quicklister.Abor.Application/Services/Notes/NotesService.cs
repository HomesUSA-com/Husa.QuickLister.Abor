namespace Husa.Quicklister.Abor.Application.Services.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Domain.Entities;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Notes.Api.Contracts.Request;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Extensions;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using NoteResponse = Husa.Notes.Api.Contracts.Response.Note;

    public abstract class NotesService<T> : INotesService
        where T : Entity
    {
        private readonly INotesClient notesClient;
        private readonly INotesBusService noteService;
        private readonly IUserRepository userRepository;
        private readonly IRepository<T> repository;

        protected NotesService(
            INotesClient notesClient,
            IUserContextProvider userContextProvider,
            INotesBusService noteService,
            IUserRepository userRepository,
            IRepository<T> repository,
            ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.notesClient = notesClient ?? throw new ArgumentNullException(nameof(notesClient));
            this.UserContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected IUserContextProvider UserContextProvider { get; }

        protected ILogger Logger { get; }

        protected abstract NoteType NoteType { get; }

        public async Task<NoteDetailResult> GetNote(Guid entityId, Guid noteId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            var note = await this.notesClient.GetNoteById(noteId);
            this.Logger.LogInformation("Found an active note with id {noteId}.", noteId);
            var noteDetailResult = ToDetailResult(note);
            await this.userRepository.FillUserNameAsync(noteDetailResult);

            return noteDetailResult;
        }

        public async Task<IEnumerable<NoteDetailResult>> GetNotes(Guid entityId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            var filter = new NoteFilter()
            {
                Type = this.NoteType.ToRemoteNoteType(),
                MarketCode = MarketCode.Austin,
                EntityId = entityId,
            };

            var notes = await this.notesClient.GetNotes(filter);
            var noteDetailResult = notes.Select(ToDetailResult);

            await this.userRepository.FillUsersNameAsync(noteDetailResult);

            return noteDetailResult;
        }

        public async Task CreateNote(Guid entityId, string title, string description)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            await this.noteService.CreateAsync(entityId, title, description, this.NoteType);
        }

        public async Task UpdateNote(Guid entityId, Guid noteId, string title, string description)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            await this.noteService.UpdateAsync(entityId, noteId, title, description, this.NoteType);
        }

        public async Task DeleteNote(Guid entityId, Guid noteId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            await this.noteService.DeleteByIdAsync(noteId);
        }

        protected async Task ValidateEntityAndUserCompany(Guid entityId)
        {
            var entity = await this.repository.GetById(entityId, filterByCompany: true) ?? throw new NotFoundException<T>(entityId);
            var user = this.UserContextProvider.GetCurrentUser();
            if (user.UserRole == UserRole.User && !user.CompanyId.Value.Equals(entity.CompanyId))
            {
                this.Logger.LogInformation("The current user logged has not allow to get the media related to the Community with id: {communityId}", entityId);
                throw new NotFoundException<T>(entityId);
            }
        }

        private static NoteDetailResult ToDetailResult(NoteResponse noteResponse) => new()
        {
            Id = noteResponse.Id,
            Title = noteResponse.Title,
            Description = noteResponse.Description,
            SysCreatedBy = noteResponse.SysCreatedBy,
            SysCreatedOn = noteResponse.SysCreatedOn,
            CreatedBy = noteResponse.CreatedBy,
            ModifiedBy = noteResponse.ModifiedBy,
            Type = noteResponse.Type.ToLocalNoteType(),
        };
    }
}
