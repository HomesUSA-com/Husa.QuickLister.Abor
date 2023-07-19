namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Migration.Api.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class PlanMigrationService : IPlanMigrationService
    {
        private readonly IPlanRepository planRepository;
        private readonly IMapper mapper;
        private readonly ILogger<PlanMigrationService> logger;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IMigrationClient migrationClient;

        public PlanMigrationService(
            IPlanRepository planRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<PlanMigrationService> logger,
            IMapper mapper)
        {
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.migrationClient = migrationClient ?? throw new ArgumentNullException(nameof(migrationClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task MigrateByCompanyId(Guid companyId)
        {
            var company = await this.serviceSubscriptionClient.Company.GetCompany(companyId);
            if (company == null || !company.LegacyId.HasValue)
            {
                throw new DomainException($"The company with id {companyId} is not associated to an id in v1");
            }

            this.logger.LogInformation("Starting to migrate plans from v1 related to company with id {companyId}.", companyId);
            var plansMigration = await this.migrationClient.Plans.GetByCompanyIdAsync(company.LegacyId.Value);

            if (!plansMigration.Any())
            {
                this.logger.LogInformation("Plans for company with id {companyId} where not found.", companyId);
                return;
            }

            var existingPlanIds = (await this.planRepository.GetByLegacyIds(plansMigration.Select(p => p.Id), companyId)).Select(x => x.LegacyId);

            var plans = new List<Plan>();
            foreach (var planMigration in plansMigration.Where(p => !existingPlanIds.Contains(p.Id)))
            {
                var basePlan = this.mapper.Map<BasePlan>(planMigration);
                basePlan.OwnerName = company.Name;
                var rooms = this.mapper.Map<IEnumerable<PlanRoom>>(planMigration.Rooms);

                var plan = new Plan(companyId, planMigration.PlanName, company.Name);
                plan.Migrate(planMigration.Id, basePlan, rooms);

                plans.Add(plan);
            }

            this.planRepository.Attach(plans);
            await this.planRepository.SaveChangesAsync();
        }
    }
}
