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
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-communities/{communityId}/notes")]
    public class SaleCommunityNotesController : Controller
    {
        private readonly ICommunityNotesService communityNotesService;
        private readonly ILogger<SaleCommunityNotesController> logger;
        private readonly IMapper mapper;
        public SaleCommunityNotesController(
            ICommunityNotesService communityNotesService,
            ILogger<SaleCommunityNotesController> logger,
            IMapper mapper)
        {
            this.communityNotesService = communityNotesService ?? throw new ArgumentNullException(nameof(communityNotesService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes([FromRoute] Guid communityId)
        {
            this.logger.LogInformation("Starting to get community note resources for the entity {communityId}", communityId);
            var resources = await this.communityNotesService.GetNotes(communityId);
            var notes = this.mapper.Map<IEnumerable<NotesResponse>>(resources);
            return this.Ok(notes);
        }

        [HttpGet("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid communityId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Starting to get the note for community entity {communityId} and note Id '{noteId}'", communityId, noteId);
            var resource = await this.communityNotesService.GetNote(communityId, noteId);
            var note = this.mapper.Map<NotesResponse>(resource);
            return this.Ok(note);
        }

        [HttpPost]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid communityId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Starting to add note to community with id {communityId}", communityId);
            await this.communityNotesService.CreateNote(communityId, note.Title, note.Description);
            return this.Ok();
        }

        [HttpDelete("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> DeleteById([FromRoute] Guid communityId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Starting to delete note fro community with id {noteId}", noteId);
            await this.communityNotesService.DeleteNote(communityId, noteId);
            return this.Ok();
        }

        [HttpPut("{noteId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid communityId, [FromRoute] Guid noteId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Starting to update note for community with id {noteId}", noteId);
            await this.communityNotesService.UpdateNote(communityId, noteId, note.Title, note.Description);
            return this.Ok();
        }
    }
}
