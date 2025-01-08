namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Extensions.JsonImport;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PlanExtensions = Husa.Quicklister.Extensions.Application.Services.JsonImport;
    public class PlanJsonImportService : PlanExtensions.PlanJsonImportService<Plan, IPlanRepository>
    {
        public PlanJsonImportService(
            IJsonImportClient client,
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<PlanJsonImportService> logger)
            : base(client, planRepository, userContextProvider, applicationOptions, logger)
        {
        }

        protected override Task<Plan> CreatePlan(PlanResponse jsonPlan, Guid companyId, string companyName)
            => Task.FromResult(new Plan(
                    companyId,
                    jsonPlan.Name,
                    companyName,
                    jsonStatus: JsonImportStatus.AwaitingApproval));

        protected override Task UpdatePlan(Plan plan, PlanResponse jsonPlan)
        {
            plan.Import(jsonPlan);
            return Task.CompletedTask;
        }
    }
}
