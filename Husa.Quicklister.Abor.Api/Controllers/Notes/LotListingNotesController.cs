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
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("lot-listings/{listingId}/notes")]
    public class LotListingNotesController : Controller
    {
        private readonly ILotListingNotesService listingNotesService;
        private readonly ILogger<LotListingNotesController> logger;
        private readonly IMapper mapper;
        public LotListingNotesController(
            ILotListingNotesService listingNotesService,
            ILogger<LotListingNotesController> logger,
            IMapper mapper)
        {
            this.listingNotesService = listingNotesService ?? throw new ArgumentNullException(nameof(listingNotesService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting to get the note resources for the entity {listingId}", listingId);

            var resources = await this.listingNotesService.GetNotes(listingId);
            var notes = this.mapper.Map<IEnumerable<NotesResponse>>(resources);
            return this.Ok(notes);
        }

        [HttpGet("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid listingId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Starting to get the note for the entity {listingId} and note Id '{noteId}'", listingId, noteId);
            var resource = await this.listingNotesService.GetNote(listingId, noteId);
            var note = this.mapper.Map<NotesResponse>(resource);
            return this.Ok(note);
        }

        [HttpPost]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid listingId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Starting to add note to entity id {listingId}", listingId);
            await this.listingNotesService.CreateNote(listingId, note.Title, note.Description);
            return this.Ok();
        }

        [HttpDelete("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid listingId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Deleting note with id {noteId} from listing {listingId}", listingId, noteId);
            await this.listingNotesService.DeleteNote(listingId, noteId);
            return this.Ok();
        }

        [HttpPut("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid listingId, [FromRoute] Guid noteId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Updating note with id {noteId} from listing {listingId}", listingId, noteId);
            await this.listingNotesService.UpdateNote(listingId, noteId, note.Title, note.Description);
            return this.Ok();
        }
    }
}
