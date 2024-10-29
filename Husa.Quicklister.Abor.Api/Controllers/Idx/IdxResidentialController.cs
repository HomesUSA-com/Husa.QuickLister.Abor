namespace Husa.Quicklister.Abor.Api.Controllers.Idx
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [AllowAnonymous]
    [Route("idx-residential")]
    public class IdxResidentialController : Controller
    {
        private readonly IResidentialIdxQueriesRepository idxQueriesRepository;
        private readonly ILogger<IdxResidentialController> logger;
        private readonly IMapper mapper;

        public IdxResidentialController(
            IResidentialIdxQueriesRepository idxQueriesRepository,
            ILogger<IdxResidentialController> logger,
            IMapper mapper)
        {
            this.idxQueriesRepository = idxQueriesRepository ?? throw new ArgumentNullException(nameof(idxQueriesRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetListings([FromQuery][Required] string builderName)
        {
            this.logger.LogInformation("Starting to get idx residentials for {builderName}", builderName);

            var result = await this.idxQueriesRepository.FindByBuilderName(builderName);
            var idxResidentials = this.mapper.Map<IEnumerable<ResidentialIdxResponse>>(result);

            return this.Ok(idxResidentials);
        }
    }
}
