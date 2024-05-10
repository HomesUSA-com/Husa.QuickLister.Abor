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
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class PlanService : IPlanService
    {
        private readonly IPlanRepository planRepository;
        private readonly ILogger<PlanService> logger;
        private readonly IMapper mapper;
        private readonly IUserContextProvider userContextProvider;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        public PlanService(
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            IMapper mapper,
            ILogger<PlanService> logger)
        {
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> CreateAsync(PlanCreateDto planDto)
        {
            var planFound = await this.planRepository.GetPlan(planDto.Name, planDto.CompanyId);
            if (planFound is not null)
            {
                throw new DomainException($"Plan '{planFound.BasePlan.Name}' for the company '{planFound.BasePlan.OwnerName}' already exists!");
            }

            this.logger.LogInformation("Creating plan profile {planName} of company {companyId}", planDto.Name, planDto.CompanyId);
            var company = await this.serviceSubscriptionClient.Company.GetCompany(planDto.CompanyId);
            var plan = new Plan(
                company.Id,
                planDto.Name,
                company.Name);

            this.planRepository.Attach(plan);
            await this.planRepository.SaveChangesAsync();
            return plan.Id;
        }

        public async Task DeletePlan(Guid planId, bool deleteInCascade = false)
        {
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.logger.LogInformation("Starting delete Plan with id {planId}", planId);
            plan.Delete(this.userContextProvider.GetCurrentUserId(), deleteInCascade);
            await this.planRepository.SaveChangesAsync(plan);
        }

        public async Task UpdatePlanAsync(Guid planId, UpdatePlanDto updatePlanDto)
        {
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.logger.LogInformation("Starting update plan profile with id {planId}", planId);
            var planFound = await this.planRepository.GetPlan(updatePlanDto.Name, updatePlanDto.CompanyId);
            if (planFound is not null && plan.Id != planFound.Id)
            {
                this.logger.LogError("Plan '{planName}' for the company '{planOwnerName}' already exists!", planFound.BasePlan.Name, planFound.BasePlan.OwnerName);
                throw new DomainException($"Plan '{planFound.BasePlan.Name}' for the company '{planFound.BasePlan.OwnerName}' already exists!");
            }

            plan.UpdateCompany(updatePlanDto.CompanyId);
            plan.UpdateBasePlanInformation(this.mapper.Map<BasePlan>(updatePlanDto));
            plan.UpdateRooms(this.mapper.Map<IEnumerable<PlanRoom>>(updatePlanDto.Rooms));
            await this.planRepository.UpdateAsync(plan);
        }

        public async Task UpdateListingsFromPlanAsync(Guid planId)
        {
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);
            this.logger.LogInformation("Starting update listing from plan with id {planId}", planId);

            foreach (var listing in plan.SaleProperties)
            {
                listing.ImportDataFromPlan(plan);
            }

            await this.planRepository.SaveChangesAsync();
        }
    }
}
