namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;
    using PhotoRequest = Husa.PhotoService.Api.Contracts.Request;

    public class PlanMigrationService : ExtensionsServices.PlanMigrationService<Plan, IPlanRepository, IPlanPhotoService>
    {
        private readonly IMapper mapper;

        public PlanMigrationService(
            IPlanRepository planRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IPlanPhotoService photoService,
            ILogger<PlanMigrationService> logger,
            IMapper mapper)
            : base(planRepository, migrationClient, serviceSubscriptionClient, photoService, logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override Plan CreatePlan(CompanyDetail company, PlanResponse planMigration)
        {
            var basePlan = this.mapper.Map<BasePlan>(planMigration);
            basePlan.OwnerName = company.Name;
            var rooms = this.mapper.Map<IEnumerable<PlanRoom>>(planMigration.Rooms);

            var plan = new Plan(company.Id, planMigration.PlanName, company.Name);
            plan.Migrate(planMigration.Id, basePlan, rooms);

            return plan;
        }

        protected override void UpdatePhotoRequestProperty(PhotoRequest.Property property, Plan plan)
        {
            property.PlanName = plan.BasePlan.Name;
            property.City = property.ReadableCity.GetEnumValueFromDescription<Cities>().ToString();
        }
    }
}
