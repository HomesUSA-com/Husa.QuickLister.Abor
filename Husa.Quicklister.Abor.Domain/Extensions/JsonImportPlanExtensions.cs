namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public static class JsonImportPlanExtensions
    {
        public static BasePlan Import(PlanResponse jsonPlan, string companyName)
        {
            ArgumentNullException.ThrowIfNull(jsonPlan);
            return new BasePlan(name: jsonPlan.Name, ownerName: companyName)
            {
                FullBathsTotal = jsonPlan.Bathrooms,
                HalfBathsTotal = jsonPlan.HalfBaths,
                StoriesTotal = jsonPlan.Stories?.ToString().ToStories(),
                MainLevelBedroomTotal = jsonPlan.Bedrooms,
                LivingAreasTotal = jsonPlan.LivingAreas,
                DiningAreasTotal = jsonPlan.DinningAreas,
            };
        }
    }
}
