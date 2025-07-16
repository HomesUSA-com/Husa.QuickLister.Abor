namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ShowingTime;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Interfaces.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class ShowingTimeContactsController : Husa.Quicklister.Extensions.Api.Controllers.ShowingTimeContactsController
    {
        private readonly IShowingTimeContactQueriesRepository showingTimeContactQueryRepository;

        public ShowingTimeContactsController(
            IShowingTimeContactService showingTimeContactService,
            IShowingTimeContactQueriesRepository showingTimeContactQueryRepository,
            ILogger<ShowingTimeContactsController> logger,
            IMapper mapper)
            : base(showingTimeContactService, logger, mapper)
        {
            this.showingTimeContactQueryRepository = showingTimeContactQueryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] ShowingTimeContactRequestFilter requestFilter)
        {
            this.Logger.LogInformation("Starting to get ShowingTime contacts in Abor");
            var queryFilter = this.Mapper.Map<ShowingTimeContactQueryFilter>(requestFilter);
            var queryResponse = await this.showingTimeContactQueryRepository.Search(queryFilter);
            var data = this.Mapper.Map<IEnumerable<ContactResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<ContactResponse>(data, queryResponse.Total));
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetAsync(Guid contactId)
        {
            this.Logger.LogInformation("Starting to get ShowingTime contact in Abor");
            var queryResponse = await this.showingTimeContactQueryRepository.GetContactById(contactId);
            var data = this.Mapper.Map<ContactDetailResponse>(queryResponse);
            return this.Ok(data);
        }
    }
}
