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
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("plans/{planId}/notes")]
    public class PlanNotesController : Controller
    {
        private readonly IPlanNotesService planNotesService;
        private readonly ILogger<PlanNotesController> logger;
        private readonly IMapper mapper;

        public PlanNotesController(
            IPlanNotesService planNotesService,
            ILogger<PlanNotesController> logger,
            IMapper mapper)
        {
            this.planNotesService = planNotesService ?? throw new ArgumentNullException(nameof(planNotesService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetNotes([FromRoute] Guid planId)
        {
            this.logger.LogInformation("Getting the note resources for the plan {planId}", planId);
            var resources = await this.planNotesService.GetNotes(planId);
            var notes = this.mapper.Map<IEnumerable<NotesResponse>>(resources);
            return this.Ok(notes);
        }

        [HttpGet("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid planId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Starting to get the note for the entity {planId} and note Id '{noteId}'", planId, noteId);
            var resources = await this.planNotesService.GetNote(planId, noteId);
            var note = this.mapper.Map<NotesResponse>(resources);
            return this.Ok(note);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid planId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Adding note to entity id {planId}", planId);
            await this.planNotesService.CreateNote(planId, title: note.Title, description: note.Description);
            return this.Ok();
        }

        [HttpDelete("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid planId, [FromRoute] Guid noteId)
        {
            this.logger.LogInformation("Deleting note with id {noteId}", noteId);
            await this.planNotesService.DeleteNote(planId, noteId);
            return this.Ok();
        }

        [HttpPut("{noteId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid planId, [FromRoute] Guid noteId, [FromBody] NoteRequest note)
        {
            this.logger.LogInformation("Updating note with id {noteId}", noteId);
            await this.planNotesService.UpdateNote(planId, noteId, title: note.Title, description: note.Description);
            return this.Ok();
        }
    }
}
