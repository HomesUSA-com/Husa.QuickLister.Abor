namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Plans;

    public class PlanDeletionService : ExtensionsServices.PlanDeletionService<Plan>
    {
        public PlanDeletionService(
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            IJsonImportClient importClient,
            ILogger<PlanDeletionService> logger)
            : base(planRepository, userContextProvider, importClient, logger)
        {
        }
    }
}
