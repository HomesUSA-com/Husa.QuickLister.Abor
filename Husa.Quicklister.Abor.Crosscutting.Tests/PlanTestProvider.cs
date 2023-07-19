namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class PlanTestProvider
    {
        public static Plan GetPlanEntity(
            Guid? planId = null,
            Guid? companyId = null)
        {
            var company = companyId ?? Guid.NewGuid();
            var id = planId ?? Guid.NewGuid();

            return new Plan(company, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
            {
                Id = id,
            };
        }

        public static UpdatePlanRequest GetUpdatePlanRequest(
            Guid? companyId = null)
        => new()
        {
            Stories = Faker.Enum.Random<Stories>(),
            CompanyId = companyId ?? Guid.NewGuid(),
            Name = Faker.Name.FullName(),
            OwnerName = Faker.Company.Name(),
        };
    }
}
