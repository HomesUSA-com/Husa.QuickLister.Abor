namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using System;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public static class JsonImportPlanExtensions
    {
        public static void Import(this Plan plan, PlanResponse jsonPlan)
        {
            var basePlan = Import(jsonPlan, companyName: plan.BasePlan.OwnerName);
            plan.UpdateBasePlanInformation(basePlan);

            var rooms = jsonPlan.Rooms.ToRooms();
            if (rooms.Count > 0)
            {
                plan.ImportRooms(rooms);
            }
        }

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
