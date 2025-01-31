namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;

    public static class ImportSalePropertyToPlanExtensions
    {
        public static void ImportFromSaleProperty(this Plan plan, SaleProperty saleProperty, bool updateRooms = true, bool overwriteFieldsOnlyIfNull = false)
        {
            if (updateRooms)
            {
                var rooms = saleProperty.Rooms.ConvertToPlanRoom();
                plan.UpdateRooms(rooms);
            }

            plan.BasePlan.ImportSpacesDimensionsFromSaleListing(saleProperty.SpacesDimensionsInfo, overwriteFieldsOnlyIfNull);
        }

        private static void ImportSpacesDimensionsFromSaleListing(this BasePlan basePlan, SpacesDimensionsInfo spacesDimensionsInfo, bool overwriteFieldsOnlyIfNull = false)
        {
            if (overwriteFieldsOnlyIfNull)
            {
                basePlan.StoriesTotal ??= spacesDimensionsInfo.StoriesTotal;
                basePlan.HalfBathsTotal ??= spacesDimensionsInfo.HalfBathsTotal;
                basePlan.FullBathsTotal ??= spacesDimensionsInfo.FullBathsTotal;
                basePlan.SqFtTotal ??= spacesDimensionsInfo.SqFtTotal;
                basePlan.MainLevelBedroomTotal ??= spacesDimensionsInfo.MainLevelBedroomTotal;
                basePlan.OtherLevelsBedroomTotal ??= spacesDimensionsInfo.OtherLevelsBedroomTotal;
                basePlan.LivingAreasTotal ??= spacesDimensionsInfo.LivingAreasTotal;
                basePlan.DiningAreasTotal ??= spacesDimensionsInfo.DiningAreasTotal;
                return;
            }

            basePlan.StoriesTotal = spacesDimensionsInfo.StoriesTotal;
            basePlan.HalfBathsTotal = spacesDimensionsInfo.HalfBathsTotal;
            basePlan.FullBathsTotal = spacesDimensionsInfo.FullBathsTotal;
            basePlan.SqFtTotal = spacesDimensionsInfo.SqFtTotal;
            basePlan.MainLevelBedroomTotal = spacesDimensionsInfo.MainLevelBedroomTotal;
            basePlan.OtherLevelsBedroomTotal = spacesDimensionsInfo.OtherLevelsBedroomTotal;
            basePlan.LivingAreasTotal = spacesDimensionsInfo.LivingAreasTotal;
            basePlan.DiningAreasTotal = spacesDimensionsInfo.DiningAreasTotal;
        }
    }
}
