namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;

    public class PlanXmlService : IPlanXmlService
    {
        private readonly ILogger<PlanXmlService> logger;
        private readonly IXmlClient xmlClient;
        private readonly IPlanRepository planRepository;

        public PlanXmlService(
            IXmlClient xmlClient,
            IPlanRepository planRepository,
            ILogger<PlanXmlService> logger)
        {
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ImportEntity(Guid companyId, string companyName, Guid entityId)
        {
            this.logger.LogInformation("Importing xml xmlPlan with id {xmlPlanId} to company with id {companyId}", entityId, companyId);
            var xmlPlan = await this.xmlClient.Plan.GetByIdAsync(entityId);
            var basePlan = BasePlan.ImportFromXml(xmlPlan, companyName: companyName);

            var plan = await GetPlan(companyId, xmlPlan, companyName);
            plan.UpdateBasePlanInformation(basePlan);

            await this.planRepository.SaveChangesAsync();
            await this.xmlClient.Plan.ProcessPlan(entityId, planRequest: new() { PlanId = plan.Id });

            async Task<Plan> GetPlan(Guid companyId, PlanResponse xmlPlan, string companyName)
            {
                var planId = xmlPlan.PlanProfileId;
                if (planId.HasValue && planId != Guid.Empty)
                {
                    return await this.planRepository.GetById(planId.Value, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
                }

                var plan = new Plan(
                                companyId,
                                name: xmlPlan.Name,
                                ownerName: companyName,
                                xmlStatus: XmlStatus.AwaitingApproval);

                this.planRepository.Attach(plan);
                return plan;
            }
        }

        public async Task ApprovePlan(Guid planId)
        {
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.logger.LogInformation("Starting approve plan with id {planId}", planId);
            plan.Approve();
            await this.planRepository.SaveChangesAsync();
            await this.xmlClient.Plan.ApprovePlanByLinkedId(planId);
        }
    }
}
