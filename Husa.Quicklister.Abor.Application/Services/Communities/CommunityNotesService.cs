namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Husa.Extensions.Authorization;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Services.Notes;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class CommunityNotesService : NotesService<CommunitySale>, ICommunityNotesService
    {
        public CommunityNotesService(
            ICommunitySaleRepository communitySaleRepository,
            IUserContextProvider userContextProvider,
            INotesBusService noteService,
            IUserRepository userRepository,
            INotesClient notesClient,
            ILogger<CommunityNotesService> logger)
            : base(notesClient, userContextProvider, noteService, userRepository, communitySaleRepository, logger)
        {
        }

        protected override NoteType NoteType => NoteType.CommunityProfile;
    }
}
