namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Microsoft.Extensions.Logging;
    using PlanExtensions = Husa.Quicklister.Extensions.Application.Services.Plans;
    public class PlanJsonImportService : PlanExtensions.PlanJsonImportService<Plan, IPlanRepository>
    {
        public PlanJsonImportService(
            IJsonImportClient client,
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            ILogger<PlanJsonImportService> logger)
            : base(client, planRepository, userContextProvider, logger)
        {
        }

        protected override async Task<Plan> CreateOrUpdatePlan(PlanResponse jsonPlan, Guid companyId, string companyName)
        {
            Plan plan;
            if (jsonPlan.QuicklisterId.HasValue && jsonPlan.QuicklisterId != Guid.Empty)
            {
                plan = await this.PlanRepository.GetById(jsonPlan.QuicklisterId.Value, filterByCompany: true) ??
                    throw new NotFoundException<Plan>(jsonPlan.QuicklisterId);
            }
            else
            {
                plan = new Plan(
                    companyId,
                    jsonPlan.Name,
                    companyName,
                    jsonStatus: JsonImportStatus.AwaitingApproval);
                this.PlanRepository.Attach(plan);
            }

            var basePlan = JsonImportPlanExtensions.Import(jsonPlan, companyName: companyName);
            plan.UpdateBasePlanInformation(basePlan);
            return plan;
        }
    }
}
