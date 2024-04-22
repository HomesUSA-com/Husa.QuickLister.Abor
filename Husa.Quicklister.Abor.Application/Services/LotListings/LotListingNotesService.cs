namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using Husa.Extensions.Authorization;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Services.Notes;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class LotListingNotesService : NotesService<LotListing>, ILotListingNotesService
    {
        public LotListingNotesService(
            ILotListingRepository listingRepository,
            IUserContextProvider userContextProvider,
            INotesBusService noteService,
            IUserRepository userRepository,
            INotesClient notesClient,
            ILogger<LotListingNotesService> logger)
            : base(notesClient, userContextProvider, noteService, userRepository, listingRepository, logger)
        {
        }

        protected override NoteType NoteType => NoteType.Lot;
    }
}
