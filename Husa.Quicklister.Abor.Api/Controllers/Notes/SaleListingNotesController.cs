namespace Husa.Quicklister.Abor.Api.Controllers.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Notes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Notes;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listings/{listingId}/notes")]
    public class SaleListingNotesController : Controller
    {
        private readonly ISaleListingNotesService listingNotesService;
        private readonly ILogger<SaleListingNotesController> logger;
        private readonly IMapper mapper;
        public SaleListingNotesController(
            ISaleListingNotesService listingNotesService,
            ILogger<SaleListingNotesController> logger,
            IMapper mapper)
        {
            this.listingNotesService = listingNotesService ?? throw new ArgumentNullException(nameof(listingNotesService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetNotes([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting to get the note resources for the entity {listingId}", listingId);

            var resources = await this.listingNotesService.GetNotes(listingId);
            var notes = this.mapper.Map<IEnumerable<NotesResponse>>(resources);
            return this.Ok(notes);
        }

        [HttpGet("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid listingId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Starting to get the note for the entity {listingId} and note Id '{noteId}'", listingId, noteId);
            var resource = await this.listingNotesService.GetNote(listingId, noteId);
            var note = this.mapper.Map<NotesResponse>(resource);
            return this.Ok(note);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid listingId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Starting to add note to entity id {listingId}", listingId);
            await this.listingNotesService.CreateNote(listingId, note.Title, note.Description);
            return this.Ok();
        }

        [HttpDelete("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid listingId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Deleting note with id {noteId} from listing {listingId}", listingId, noteId);
            await this.listingNotesService.DeleteNote(listingId, noteId);
            return this.Ok();
        }

        [HttpPut("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid listingId, [FromRoute] Guid noteId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Updating note with id {noteId} from listing {listingId}", listingId, noteId);
            await this.listingNotesService.UpdateNote(listingId, noteId, note.Title, note.Description);
            return this.Ok();
        }
    }
}
