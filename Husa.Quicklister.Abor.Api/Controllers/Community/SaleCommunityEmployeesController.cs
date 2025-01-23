namespace Husa.Quicklister.Abor.Api.Controllers.Community
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Microsoft.Extensions.Logging;
    using ExtensionController = Husa.Quicklister.Extensions.Api.Controllers.Community;

    public class SaleCommunityEmployeesController : ExtensionController.SaleCommunityEmployeesController
    {
        public SaleCommunityEmployeesController(
            ISaleCommunityService communityService,
            IQueryCommunityEmployeeRepository queryCommunityRepository,
            ILogger<SaleCommunityEmployeesController> logger,
            IMapper mapper)
            : base(communityService, queryCommunityRepository, logger, mapper)
        {
        }
    }
}
