namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Extensions.Api.Contracts.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ShowingTime;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Interfaces.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("showing-time-contacts")]
    public class ShowingTimeContactsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<ShowingTimeContactsController> logger;
        private readonly IShowingTimeContactService showingTimeContactService;
        private readonly IShowingTimeContactQueriesRepository showingTimeContactQueryRepository;

        public ShowingTimeContactsController(
            IShowingTimeContactService showingTimeContactService,
            IShowingTimeContactQueriesRepository showingTimeContactQueryRepository,
            ILogger<ShowingTimeContactsController> logger,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.showingTimeContactService = showingTimeContactService;
            this.showingTimeContactQueryRepository = showingTimeContactQueryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] ShowingTimeContactRequestFilter requestFilter)
        {
            this.logger.LogInformation("Starting to get ShowingTime contacts in DFW");
            var queryFilter = this.mapper.Map<ShowingTimeContactQueryFilter>(requestFilter);
            var queryResponse = await this.showingTimeContactQueryRepository.GetAsync(queryFilter);
            var data = this.mapper.Map<IEnumerable<ContactResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<ContactResponse>(data, queryResponse.Total));
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetAsync(Guid contactId)
        {
            this.logger.LogInformation("Starting to get ShowingTime contact in DFW");
            var queryResponse = await this.showingTimeContactQueryRepository.GetContactById(contactId);
            var data = this.mapper.Map<ContactDetailInfo>(queryResponse);
            return this.Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ContactRequest request)
        {
            this.logger.LogInformation("Starting to CREATE a ShowingTime contacts in DFW");
            var contactDto = this.mapper.Map<ContactDto>(request);
            var contactId = await this.showingTimeContactService.CreateAsync(contactDto);
            return this.Ok(contactId);
        }

        [HttpPut("{contactId}")]
        public async Task<IActionResult> UpdateAsync(Guid contactId, [FromBody] ContactRequest request)
        {
            this.logger.LogInformation("Starting to UPDATE a ShowingTime contacts in DFW");
            var contactDto = this.mapper.Map<ContactDto>(request);
            await this.showingTimeContactService.UpdateAsync(contactId, contactDto);
            return this.Ok(contactId);
        }

        [HttpDelete("{contactId}")]
        public async Task<IActionResult> DeleteAsync(Guid contactId)
        {
            this.logger.LogInformation("Starting to DELETE a ShowingTime contacts in DFW");
            await this.showingTimeContactService.DeleteAsync(contactId);
            return this.Ok(contactId);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignContactAsync([FromBody] AssignContactRequest request)
        {
            this.logger.LogInformation("Starting to ASSIGN ShowingTimeContact to {Scope} DFW", request.Scope);
            var contactDto = this.mapper.Map<AssignContactDto>(request);
            await this.showingTimeContactService.AssignTo(contactDto);
            return this.Ok();
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveContactAsync([FromBody] AssignContactRequest request)
        {
            this.logger.LogInformation("Starting to REMOVE ShowingTimeContact to {Scope} DFW", request.Scope);
            var contactDto = this.mapper.Map<AssignContactDto>(request);
            await this.showingTimeContactService.RemoveFrom(contactDto);
            return this.Ok();
        }
    }
}
