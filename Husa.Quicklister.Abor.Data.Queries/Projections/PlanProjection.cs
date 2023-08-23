namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public static class PlanProjection
    {
        public static Expression<Func<Plan, PlanQueryResult>> ProjectionToPlanQueryResult => plan => new()
        {
            Id = plan.Id,
            CompanyId = plan.CompanyId,
            Name = plan.BasePlan.Name,
            Market = MarketCode.Austin,
            OwnerName = plan.BasePlan.OwnerName,
            SysCreatedBy = plan.SysCreatedBy,
            SysCreatedOn = plan.SysCreatedOn,
            SysModifiedBy = plan.SysModifiedBy,
            SysModifiedOn = plan.SysModifiedOn,
        };

        public static Expression<Func<Plan, PlanDetailQueryResult>> ProjectionToPlanDetailQueryResult => plan => new()
        {
            Id = plan.Id,
            Name = plan.BasePlan.Name,
            CompanyId = plan.CompanyId,
            OwnerName = plan.BasePlan.OwnerName,
            IsNewConstruction = plan.BasePlan.IsNewConstruction,
            Rooms = plan.Rooms.ToProjectionRooms(),
            SysCreatedBy = plan.SysCreatedBy,
            SysCreatedOn = plan.SysCreatedOn,
            SysModifiedBy = plan.SysModifiedBy,
            SysModifiedOn = plan.SysModifiedOn,
            XmlStatus = plan.XmlStatus,

            StoriesTotal = plan.BasePlan.StoriesTotal,
            SqFtTotal = plan.BasePlan.SqFtTotal,
            DiningAreasTotal = plan.BasePlan.DiningAreasTotal,
            MainLevelBedroomTotal = plan.BasePlan.MainLevelBedroomTotal,
            OtherLevelsBedroomTotal = plan.BasePlan.OtherLevelsBedroomTotal,
            HalfBathsTotal = plan.BasePlan.HalfBathsTotal,
            FullBathsTotal = plan.BasePlan.FullBathsTotal,
            LivingAreasTotal = plan.BasePlan.LivingAreasTotal,
        };
    }
}
