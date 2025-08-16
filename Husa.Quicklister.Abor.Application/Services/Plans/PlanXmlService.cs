namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using PlanExtensions = Husa.Quicklister.Extensions.Application.Services.Plans;

    public class PlanXmlService : PlanExtensions.PlanXmlService<Plan, IPlanRepository>, IPlanXmlService
    {
        public PlanXmlService(
            IXmlClient xmlClient,
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            ILogger<PlanXmlService> logger)
            : base(xmlClient, planRepository, userContextProvider, logger)
        {
        }

        public override async Task ImportEntity(Guid companyId, string companyName, Guid entityId, bool selfApprove = false)
        {
            this.Logger.LogInformation("Importing xml xmlPlan with id {xmlPlanId} to company with id {companyId}", entityId, companyId);
            var xmlPlan = await this.XmlClient.Plan.GetByIdAsync(entityId);
            var basePlan = BasePlan.ImportFromXml(xmlPlan, companyName: companyName);

            var plan = await this.CreatePlan(xmlPlan, companyId, companyName);
            plan.UpdateBasePlanInformation(basePlan);

            await this.PlanRepository.SaveChangesAsync();

            if (selfApprove)
            {
                if (plan.XmlStatus != XmlStatus.NotFromXml && plan.XmlStatus != XmlStatus.Approved)
                {
                    plan.Approve();
                    await this.PlanRepository.SaveChangesAsync();
                }

                await this.XmlClient.Plan.SelfApprovePlan(entityId, plan.Id);
            }
            else
            {
                await this.XmlClient.Plan.ProcessPlan(entityId, planRequest: new() { PlanId = plan.Id });
            }
        }

        protected override async Task<Plan> CreatePlan(PlanResponse plan, Guid companyId, string companyName)
        {
            if (plan.PlanProfileId.HasValue && plan.PlanProfileId != Guid.Empty)
            {
                return await this.PlanRepository.GetById(plan.PlanProfileId.Value, filterByCompany: true) ??
                    throw new NotFoundException<Plan>(plan.PlanProfileId);
            }

            var newPlan = new Plan(
                companyId,
                plan.Name,
                companyName,
                xmlStatus: XmlStatus.AwaitingApproval);

            this.PlanRepository.Attach(newPlan);
            return newPlan;
        }
    }
}
