namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Husa.Extensions.Authorization;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Services.Notes;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class SaleListingNotesService : NotesService<SaleListing>, ISaleListingNotesService
    {
        public SaleListingNotesService(
            IListingSaleRepository listingSaleRepository,
            IUserContextProvider userContextProvider,
            INotesBusService noteService,
            IUserRepository userRepository,
            INotesClient notesClient,
            ILogger<SaleListingNotesService> logger)
            : base(notesClient, userContextProvider, noteService, userRepository, listingSaleRepository, logger)
        {
        }

        protected override NoteType NoteType => NoteType.Residential;
    }
}
