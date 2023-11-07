namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Husa.Extensions.Authorization;
    using Husa.Notes.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Services.Notes;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class PlanNotesService : NotesService<Plan>, IPlanNotesService
    {
        public PlanNotesService(
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            INotesBusService noteService,
            IUserRepository userRepository,
            INotesClient notesClient,
            ILogger<PlanNotesService> logger)
            : base(notesClient, userContextProvider, noteService, userRepository, planRepository, logger)
        {
        }

        protected override NoteType NoteType => NoteType.PlanProfile;
    }
}
