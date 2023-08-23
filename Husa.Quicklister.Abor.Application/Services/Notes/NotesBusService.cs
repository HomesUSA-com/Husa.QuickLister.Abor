namespace Husa.Quicklister.Abor.Application.Services.Notes
{
    using System;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Notes.ServiceBus.Contracts;
    using Husa.Notes.ServiceBus.Contracts.Contracts;
    using Husa.Quicklister.Abor.Application.Extensions;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class NotesBusService : MessagingServiceBusBase, INotesBusService
    {
        private readonly ILogger<NotesBusService> logger;
        private readonly IUserContextProvider userContextProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NotesBusService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            ServiceBusClient client,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NotesBusService> logger)
            : base(logger, client, serviceBusSettings.Value.NotesService.TopicName)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task UpdateAsync(Guid noteId, NoteMessage note)
        {
            var userId = this.userContextProvider.GetCurrentUserId();
            var correlationId = this.httpContextAccessor.HttpContext.TraceIdentifier;
            note.MarketCode = MarketCode.Austin;
            var noteMessage = new NoteUpdatedMessage
            {
                Id = noteId,
                Note = note,
            };
            this.logger.LogInformation("Requesting the update of note {@note} by user {userId}", note, userId);
            await this.SendMessage(messages: new[] { noteMessage }, userId: userId.ToString(), correlationId: correlationId);
        }

        public async Task DeleteByIdAsync(Guid noteId)
        {
            var userId = this.userContextProvider.GetCurrentUserId();
            var correlationId = this.httpContextAccessor.HttpContext.TraceIdentifier;
            var noteMessage = new NoteDeletedMessage { Id = noteId };
            this.logger.LogInformation("Requesting the update of note {@note} by user {userId}", noteMessage, userId);
            await this.SendMessage(messages: new[] { noteMessage }, userId: userId.ToString(), correlationId: correlationId);
        }

        public async Task CreateAsync(Guid entityId, string title, string description, NoteType noteType)
        {
            var note = new NoteMessage
            {
                Title = title,
                Description = description,
                EntityId = entityId,
                Type = noteType.ToRemoteNoteType(),
                MarketCode = MarketCode.Austin,
            };
            var noteMessage = new NoteCreatedMessage { Note = note };
            var userId = this.userContextProvider.GetCurrentUserId();
            var correlationId = this.httpContextAccessor.HttpContext.TraceIdentifier;
            this.logger.LogInformation("Requesting the creation of note {noteType} for entity {entityId} by user {userId}", noteType, entityId, userId);
            await this.SendMessage(messages: new[] { noteMessage }, userId.ToString(), correlationId: correlationId);
        }

        public async Task UpdateAsync(Guid entityId, Guid noteId, string title, string description, NoteType noteType)
        {
            var note = new NoteMessage
            {
                Id = noteId,
                Title = title,
                Description = description,
                EntityId = entityId,
                Type = noteType.ToRemoteNoteType(),
                MarketCode = MarketCode.Austin,
            };
            var noteMessage = new NoteUpdatedMessage
            {
                Id = noteId,
                Note = note,
            };

            var userId = this.userContextProvider.GetCurrentUserId();
            var correlationId = this.httpContextAccessor.HttpContext.TraceIdentifier;
            this.logger.LogInformation("Requesting the update of note {noteId} by user {userId}", noteId, userId);
            await this.SendMessage(messages: new[] { noteMessage }, userId: userId.ToString(), correlationId: correlationId);
        }
    }
}
