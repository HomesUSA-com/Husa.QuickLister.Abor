namespace Husa.Quicklister.Abor.Data.Commands.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using LogExtensions = Husa.Quicklister.Extensions.Data.Documents.Extensions.ListingChangesLogExtensions;

    public static class ListingChangesLogExtensions
    {
        public static void ProcessEntityChanges(List<SummaryField> fields, SaleListing originalEntity, SaleListing updatedEntity)
        {
            var showingTimeKeys = LogExtensions.ProcessShowingTimeChanges(fields, originalEntity, updatedEntity);
            var entityPairs = new Dictionary<string, (object Original, object Updated)>
                {
                    { nameof(SaleListing.StatusFieldsInfo), (originalEntity.StatusFieldsInfo, updatedEntity.StatusFieldsInfo) },
                    { nameof(SaleListing.InvoiceInfo), (originalEntity.InvoiceInfo, updatedEntity.InvoiceInfo) },
                    { nameof(SaleListing.PublishInfo), (originalEntity.PublishInfo, updatedEntity.PublishInfo) },
                };
            fields.AddSectionProperties(entityPairs);

            var ignoreRootProperties = entityPairs.Keys.Concat(showingTimeKeys).Concat([nameof(SaleListing.SaleProperty)]);
            fields.AddProperties(
                originalEntity,
                updatedEntity,
                excludeFields: ignoreRootProperties.ToArray());
        }

        public static void ProcessSalePropertyChanges(List<SummaryField> fields, SaleProperty originalProperty, SaleProperty updatedProperty)
        {
            var salePropertyPrefix = $"{nameof(SaleListing.SaleProperty)}.";
            fields.AddProperties(
                originalProperty,
                updatedProperty,
                salePropertyPrefix,
                filterFields: [nameof(SaleProperty.CommunityId), nameof(SaleProperty.OwnerName), nameof(SaleProperty.PlanId)]);
            var propertyPairs = new Dictionary<string, (object Original, object Updated)>
                {
                    { nameof(SaleProperty.AddressInfo), (originalProperty?.AddressInfo, updatedProperty.AddressInfo) },
                    { nameof(SaleProperty.PropertyInfo), (originalProperty?.PropertyInfo, updatedProperty.PropertyInfo) },
                    { nameof(SaleProperty.ShowingInfo), (originalProperty?.ShowingInfo, updatedProperty.ShowingInfo) },
                    { nameof(SaleProperty.SchoolsInfo), (originalProperty?.SchoolsInfo, updatedProperty.SchoolsInfo) },
                    { nameof(SaleProperty.FeaturesInfo), (originalProperty?.FeaturesInfo, updatedProperty.FeaturesInfo) },
                    { nameof(SaleProperty.FinancialInfo), (originalProperty?.FinancialInfo, updatedProperty.FinancialInfo) },
                    { nameof(SaleProperty.SpacesDimensionsInfo), (originalProperty?.SpacesDimensionsInfo, updatedProperty.SpacesDimensionsInfo) },
                };
            fields.AddSectionProperties(propertyPairs, salePropertyPrefix);
            fields.AddPropertiesWithComparer<ListingSaleRoom, ListingRoomComparer>(
                updatedProperty.Rooms,
                originalProperty?.Rooms,
                $"{salePropertyPrefix}Rooms",
                filterFields: typeof(IProvideRoomInfo).GetProperties().Select(p => p.Name).ToArray());
            fields.AddPropertiesWithComparer<SaleListingOpenHouse, OpenHouseComparer>(
                updatedProperty.OpenHouses,
                originalProperty?.OpenHouses,
                $"{salePropertyPrefix}OpenHouses",
                filterFields: typeof(IProvideOpenHouseInfo).GetProperties().Select(p => p.Name).ToArray());
        }
    }
}
