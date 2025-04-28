namespace Husa.Quicklister.Abor.Api.Controllers.Notes
{
    using AutoMapper;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Microsoft.Extensions.Logging;

    public class LotListingNotesController : Husa.Quicklister.Extensions.Api.Controllers.Notes.LotListingNotesController
    {
        public LotListingNotesController(
            ILotListingNotesService listingNotesService,
            ILogger<LotListingNotesController> logger,
            IMapper mapper)
            : base(listingNotesService, logger, mapper)
        {
        }
    }
}
