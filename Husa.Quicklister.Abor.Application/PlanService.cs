namespace Husa.Quicklister.Abor.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Plans;

    public class PlanService : ExtensionsServices.PlanService<Plan, IPlanRepository>, IPlanService
    {
        private readonly IMapper mapper;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        public PlanService(
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            IMapper mapper,
            ILogger<PlanService> logger)
            : base(planRepository, userContextProvider, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> CreateAsync(PlanCreateDto planDto)
        {
            var planFound = await this.PlanRepository.GetPlan(planDto.Name, planDto.CompanyId);
            if (planFound is not null)
            {
                throw new DomainException($"Plan '{planFound.BasePlan.Name}' for the company '{planFound.BasePlan.OwnerName}' already exists!");
            }

            this.Logger.LogInformation("Creating plan profile {planName} of company {companyId}", planDto.Name, planDto.CompanyId);
            var company = await this.serviceSubscriptionClient.Company.GetCompany(planDto.CompanyId);
            var plan = new Plan(
                company.Id,
                planDto.Name,
                company.Name);

            this.PlanRepository.Attach(plan);
            await this.PlanRepository.SaveChangesAsync();
            return plan.Id;
        }

        public async Task UpdatePlanAsync(Guid planId, UpdatePlanDto updatePlanDto)
        {
            var plan = await this.PlanRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.Logger.LogInformation("Starting update plan profile with id {planId}", planId);
            var planFound = await this.PlanRepository.GetPlan(updatePlanDto.Name, updatePlanDto.CompanyId);
            if (planFound is not null && plan.Id != planFound.Id)
            {
                this.Logger.LogError("Plan '{planName}' for the company '{planOwnerName}' already exists!", planFound.BasePlan.Name, planFound.BasePlan.OwnerName);
                throw new DomainException($"Plan '{planFound.BasePlan.Name}' for the company '{planFound.BasePlan.OwnerName}' already exists!");
            }

            plan.UpdateCompany(updatePlanDto.CompanyId);
            plan.UpdateBasePlanInformation(this.mapper.Map<BasePlan>(updatePlanDto));
            plan.UpdateRooms(this.mapper.Map<IEnumerable<PlanRoom>>(updatePlanDto.Rooms));
            await this.PlanRepository.UpdateAsync(plan);
        }

        public async Task UpdateListingsAsync(Guid planId)
        {
            var plan = await this.PlanRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.Logger.LogInformation("Starting update listing from plan with id {planId}", planId);
            plan.UpdateListingsFromPlanAsync<SaleListing, Plan>();
            await this.PlanRepository.SaveChangesAsync();
        }
    }
}
