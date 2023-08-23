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
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class ListingSaleSpecifications
    {
        public static IQueryable<T> FilterByStatus<T>(this IQueryable<T> listings, IEnumerable<MarketStatuses> marketStatus)
            where T : Listing
        {
            if (marketStatus.Any())
            {
                return listings.Where(p => marketStatus.Contains(p.MlsStatus));
            }

            return listings;
        }

        public static IQueryable<T> FilterByListed<T>(this IQueryable<T> listings, ListedType? listedType)
            where T : Listing
        {
            if (!listedType.HasValue)
            {
                return listings;
            }

            if (listedType == ListedType.Unlisted)
            {
                return listings.Where(p => string.IsNullOrEmpty(p.MlsNumber));
            }
            else if (listedType == ListedType.Listed)
            {
                return listings.Where(p => !string.IsNullOrEmpty(p.MlsNumber));
            }
            else if (listedType == ListedType.AwaitingMlsUpdate)
            {
                return listings.Where(p => p.LockedStatus == LockedStatus.LockedByUser || p.LockedStatus == LockedStatus.LockedBySystem);
            }

            return listings.Where(p => p.MlsStatus != MarketStatuses.Sold);
        }

        public static IQueryable<T> FilterByCommunity<T>(this IQueryable<T> listings, Guid? communityId)
            where T : SaleListing
        {
            if (communityId.HasValue && communityId != Guid.Empty)
            {
                return listings.Where(p => p.SaleProperty.CommunityId == communityId);
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
                return listings.Where(l => l.PublishInfo.PublishType != null);
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
            where T : SaleListing
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
    }
}
