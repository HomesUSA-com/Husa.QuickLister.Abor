namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class ListingSaleSpecifications
    {
        public static IQueryable<T> FilterByStatus<T>(this IQueryable<T> listings, IEnumerable<MarketStatuses> marketStatus)
            where T : Listing
        {
            if (marketStatus != null && marketStatus.Any())
            {
                return listings.Where(p => marketStatus.Contains(p.MlsStatus));
            }

            return listings;
        }

        public static IQueryable<T> FilterByActiveAndPendingWithShowOpenHousesPendingActive<T>(this IQueryable<T> listings)
     where T : SaleListing
        {
            var activeStatuses = new List<MarketStatuses>()
            {
                MarketStatuses.Active,
                MarketStatuses.ActiveUnderContract,
                MarketStatuses.Pending,
            };

            var listingsActiveAndPending = listings.Where(p => activeStatuses.Contains(p.MlsStatus));
            var listingsActiveAndPendingWithShowOpenHousesPendingActive = listingsActiveAndPending.Where(listing => listing.MlsStatus != MarketStatuses.Pending ||
                (listing.MlsStatus == MarketStatuses.Pending && listing.SaleProperty.ShowingInfo.ShowOpenHousesPending));

            return listingsActiveAndPendingWithShowOpenHousesPendingActive;
        }

        public static IQueryable<T> FilterByActiveOrPendingStatus<T>(this IQueryable<T> listings)
            where T : SaleListing
                => listings.Where(p => MarketStatusesExtensions.ActiveAndPendingStatusesForDiscrepancyReport.Contains(p.MlsStatus));

        public static IQueryable<T> WithPriceAbove<T>(this IQueryable<T> listings, decimal threshold = 0)
            where T : SaleListing
                => listings.Where(p => p.ListPrice > threshold);

        public static IQueryable<T> FilterByCommunities<T>(this IQueryable<T> listings, IEnumerable<Guid> communityIds)
           where T : SaleListing
        {
            if (communityIds != null && communityIds.Any())
            {
                return listings.Where(p => communityIds.Contains((Guid)p.SaleProperty.CommunityId));
            }

            return listings;
        }

        public static IQueryable<T> FilterByPlan<T>(this IQueryable<T> listings, Guid? planId)
            where T : SaleListing
        {
            if (planId.HasValue && planId != Guid.Empty)
            {
                return listings.Where(p => p.SaleProperty.PlanId == planId);
            }

            return listings;
        }

        public static IQueryable<T> FilterBySearch<T>(this IQueryable<T> listings, string searchBy)
            where T : SaleListing
        {
            if (string.IsNullOrEmpty(searchBy))
            {
                return listings;
            }

            var splitted = searchBy.Split(' ', 2);
            if (splitted.Length > 1)
            {
                return listings.Where(l => (l.SaleProperty.AddressInfo.StreetName.StartsWith(splitted[1]) &&
                            l.SaleProperty.AddressInfo.StreetNumber.Equals(splitted[0])) ||
                            l.SaleProperty.AddressInfo.StreetName.StartsWith(searchBy));
            }

            return listings.Where(l => l.SaleProperty.AddressInfo.StreetName.StartsWith(searchBy) ||
                                        l.SaleProperty.AddressInfo.StreetNumber.Equals(searchBy) ||
                                        l.SaleProperty.AddressInfo.Subdivision.StartsWith(searchBy) ||
                                        l.MlsNumber.Equals(searchBy) ||
                                        l.SaleProperty.AddressInfo.ZipCode.Equals(searchBy));
        }

        public static IQueryable<T> FilterByBillingType<T>(this IQueryable<T> listings, ActionType? actionType = null)
            where T : SaleListing
        {
            if (actionType == null || actionType == ActionType.All)
            {
                return listings.Where(l => l.PublishInfo.PublishType != null && l.PublishInfo.PublishType != ActionType.Migrated);
            }

            return listings.Where(l => l.PublishInfo.PublishType == actionType);
        }

        public static IQueryable<T> FilterByBillingDate<T>(this IQueryable<T> listings, DateTime from, DateTime to)
            where T : SaleListing
        {
            var centralFrom = from.ConvertToCentral();
            var centralTo = to.ConvertToCentral();

            return listings.Where(l => l.PublishInfo.PublishDate >= centralFrom && l.PublishInfo.PublishDate <= centralTo);
        }

        public static IQueryable<T> FilterByCreationRange<T>(this IQueryable<T> listings, DateTime? startDate = null, DateTime? endDate = null)
            where T : Listing
        {
            Expression<Func<T, bool>> expression = startDate.HasValue ? l => l.SysCreatedOn >= startDate.Value : null;

            if (endDate.HasValue)
            {
                Expression<Func<T, bool>> endExp = l => l.SysCreatedOn <= endDate.Value;
                expression = expression != null ? expression.And(endExp) : endExp;
            }

            return expression != null ? listings.Where(expression) : listings;
        }

        public static IQueryable<T> FilterByMlsNumber<T>(this IQueryable<T> listings, string mlsNumber)
            where T : Listing
        {
            return string.IsNullOrWhiteSpace(mlsNumber) ? listings : listings.Where(l => l.MlsNumber.StartsWith(mlsNumber));
        }

        public static IQueryable<T> FilterBySqftRange<T>(this IQueryable<T> listings, int? min = null, int? max = null)
            where T : SaleListing
        {
            Expression<Func<T, bool>> expression = min.HasValue ? l => l.SaleProperty.SpacesDimensionsInfo.SqFtTotal >= min.Value : null;

            if (max.HasValue)
            {
                Expression<Func<T, bool>> maxExp = l => l.SaleProperty.SpacesDimensionsInfo.SqFtTotal <= max.Value;
                expression = expression != null ? expression.And(maxExp) : maxExp;
            }

            return expression != null ? listings.Where(expression) : listings;
        }

        public static IQueryable<SaleListing> FilterByStreetNumber(this IQueryable<SaleListing> listings, string streetNumber)
        {
            return !string.IsNullOrWhiteSpace(streetNumber) ?
              listings.Where(listingSale => listingSale.SaleProperty.AddressInfo.StreetNumber.StartsWith(streetNumber)) :
              listings;
        }

        public static IQueryable<SaleListing> FilterByStreetName(this IQueryable<SaleListing> listings, string streetName)
        {
            return !string.IsNullOrWhiteSpace(streetName) ?
              listings.Where(listingSale => listingSale.SaleProperty.AddressInfo.StreetName.StartsWith(streetName)) :
              listings;
        }

        public static IQueryable<SaleListing> FilterByIsCompleteHome(this IQueryable<SaleListing> listings, bool? isCompleteHome)
        {
            if (!isCompleteHome.HasValue)
            {
                return listings;
            }

            Expression<Func<SaleListing, bool>> filter = listingSale =>
                listingSale.SaleProperty.PropertyInfo.ConstructionStage != ConstructionStage.Complete;

            if (isCompleteHome.Value)
            {
                filter = listingSale => listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete;
            }

            return listings.Where(filter);
        }

        public static IQueryable<SaleListing> HasOpenHouse(this IQueryable<SaleListing> listings) => listings.Where(listingSale => listingSale.SaleProperty.OpenHouses.Any());

        public static Func<T, bool> FilterByMlsStatusNotEqualTo<T>(MarketStatuses marketStatus)
            where T : SaleListing
                => listing => listing.MlsStatus != marketStatus;
    }
}
