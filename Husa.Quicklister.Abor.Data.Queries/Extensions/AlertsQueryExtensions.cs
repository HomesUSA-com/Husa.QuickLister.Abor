namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Microsoft.EntityFrameworkCore;

    public static class AlertsQueryExtensions
    {
        public static readonly IEnumerable<AlertType> AlertsWithCustomQueries = new[]
        {
            AlertType.ActiveEmployees,
            AlertType.PastDuePhotoRequests,
            AlertType.CompletedHomesWithoutPhotoRequest,
        };
        private static readonly DateTime FirstDayCurrentMonth = new(year: DateTime.Today.Year, month: DateTime.Today.Month, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);

        public static IReadOnlyDictionary<AlertType, Expression<Func<SaleListing, bool>>> AlertsDictionary { get; } = new Dictionary<AlertType, Expression<Func<SaleListing, bool>>>
        {
            { AlertType.PastDueEstimatedClosingDate, PastDueEstimatedClosingDateExpression },
            { AlertType.PastDueEstimatedCompletionDate, PastDueCompletionDateExpression },
            { AlertType.AgentBonusExpirationDate, AgentBonusExpirationDateExpression },
            { AlertType.AgentBonusExpirationDateOrLess, AgentBonusExpirationDateOrLessExpression },
            { AlertType.LockedListings, LockedListingsExpression },
            { AlertType.NotListedInMls, NotListedInMlsExpression },
            { AlertType.TempOffMarketBackOnMarket, TempOffMarketBackOnMarketExpression },
            { AlertType.TempOffMarketBackOnMarketDaysOrLess, TempOffMarketBackOnMarketDaysOrLessExpression },
            { AlertType.EstimatedClosingDaysOrLess, EstimatedClosingDaysOrLessExpression },
            { AlertType.CompletionDateDueDaysOrLess, CompletionDateDueDaysOrLessExpression },
            { AlertType.InadequatePublicRemarks, InadequatePublicRemarksExpression },
            { AlertType.ExpiringListings, ExpiringListingsExpression },
            { AlertType.CurrentDaysOnMarketOverDays, CurrentDaysOnMarketOverDaysExpression },
            { AlertType.OrphanListings, OrphanListingsExpression },
            { AlertType.ActiveAndPendingListing, ActiveAndPendingListingsExpression },
            { AlertType.ComparableAndRelistListing, ComparableAndRelistListingsExpression },
        };

        // Temp Off Market - Back on Market (BOM) Date - Due in 7 days or Less
        public static Expression<Func<SaleListing, bool>> TempOffMarketBackOnMarketDaysOrLessExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.MlsStatus == MarketStatuses.Withdrawn &&
            listingSale.StatusFieldsInfo.OffMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.Value > DateTime.UtcNow;

        // Temp Off Market - Back on Market (BOM) Date - Past Due
        public static Expression<Func<SaleListing, bool>> TempOffMarketBackOnMarketExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.MlsStatus == MarketStatuses.Withdrawn &&
            listingSale.StatusFieldsInfo.OffMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.Value <= DateTime.UtcNow;

        // Active and Pending Listings
        public static Expression<Func<SaleListing, bool>> ActiveAndPendingListingsExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus);

        // Comparable and Relist Listings
        public static Expression<Func<SaleListing, bool>> ComparableAndRelistListingsExpression => listingSale =>
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            Listing.RelistAndComparable.Contains(listingSale.PublishInfo.PublishType.Value) &&
            listingSale.PublishInfo != null &&
            listingSale.PublishInfo.PublishDate > FirstDayCurrentMonth;

        // Expiring Listings
        public static Expression<Func<SaleListing, bool>> ExpiringListingsExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.ExpirationDate.HasValue &&
            listingSale.ExpirationDate.Value > DateTime.UtcNow.AddYears(SaleListing.YearsInThePast) &&
            listingSale.ExpirationDate.Value < DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysInTheFuture);

        // Past Due Estimated Closing Date
        public static Expression<Func<SaleListing, bool>> PastDueEstimatedClosingDateExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.PendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.StatusFieldsInfo.EstimatedClosedDate.HasValue &&
            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value <= DateTime.UtcNow;

        // Estimated Closing 7 Days or Less
        public static Expression<Func<SaleListing, bool>> EstimatedClosingDaysOrLessExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.PendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.StatusFieldsInfo.EstimatedClosedDate.HasValue &&
            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value > DateTime.UtcNow &&
            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture);

        // Past Due Estimated Completion Date
        public static Expression<Func<SaleListing, bool>> PastDueCompletionDateExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Incomplete &&
            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow;

        // Completion Date Due in 7 days or Less
        public static Expression<Func<SaleListing, bool>> CompletionDateDueDaysOrLessExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Incomplete &&
            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value > DateTime.UtcNow &&
            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture);

        // Agent Bonus Expiration Date - Expired
        public static Expression<Func<SaleListing, bool>> AgentBonusExpirationDateExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.SaleProperty.FinancialInfo.HasBonusWithAmount &&
            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate <= DateTime.UtcNow;

        // Locked Listings
        public static Expression<Func<SaleListing, bool>> LockedListingsExpression => listingSale => Listing.LockedListingStatuses.Contains(listingSale.LockedStatus);

        // Not Listed in MLS
        public static Expression<Func<SaleListing, bool>> NotListedInMlsExpression => listingSale => string.IsNullOrEmpty(listingSale.MlsNumber) && listingSale.MlsStatus == MarketStatuses.Active;

        // Agent Bonus Expiration Date - 7 days or Less
        public static Expression<Func<SaleListing, bool>> AgentBonusExpirationDateOrLessExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
            listingSale.SaleProperty.FinancialInfo.HasBonusWithAmount &&
            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate > DateTime.UtcNow &&
            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture);

        // Current Days on Market Over 180 Days
        public static Expression<Func<SaleListing, bool>> CurrentDaysOnMarketOverDaysExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.MarketModifiedOn != null &&
            listingSale.DOM != null &&
            (listingSale.DOM.Value + EF.Functions.DateDiffDay(listingSale.MarketModifiedOn.Value, DateTime.UtcNow)) > SaleListing.MaxDaysInMarket &&
            listingSale.MlsStatus == MarketStatuses.Active;

        // Inadequate Public Remarks
        public static Expression<Func<SaleListing, bool>> InadequatePublicRemarksExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            SaleListing.ActiveListingStatuses.Contains(listingSale.MlsStatus) &&
            !string.IsNullOrEmpty(listingSale.SaleProperty.FeaturesInfo.PropertyDescription) &&
            listingSale.SaleProperty.FeaturesInfo.PropertyDescription.Length < SaleListing.MinPropertyDescriptionLength;

        // Orphan Listings
        public static Expression<Func<SaleListing, bool>> OrphanListingsExpression => listingSale =>
            listingSale.SaleProperty.Community == null &&
            SaleListing.OrphanListingStatuses.Contains(listingSale.MlsStatus);

        public static IQueryable<SaleListing> FilterByAlerts(
            this IQueryable<SaleListing> listingsQuery,
            IEnumerable<Expression<Func<SaleListing, bool>>> alertFilterExpressions)
        {
            Expression<Func<SaleListing, bool>> alertsFilter = null;
            if (!alertFilterExpressions.Any())
            {
                throw new InvalidOperationException("Filters list can not be empty");
            }

            foreach (var expression in alertFilterExpressions)
            {
                alertsFilter = alertsFilter == null ? expression : alertsFilter.Or(expression);
            }

            return listingsQuery.Where(alertsFilter);
        }

        public static AlertTotalProjection SelectAlertTotalsOrDefault(this IQueryable<IGrouping<int, SaleListing>> query, IEnumerable<AlertType> alerts, bool isMlsAdministrator) => query
            .Select(groupedListings => new AlertTotalProjection
            {
                TempOffMarketBackOnMarketDaysOrLess = alerts.Contains(AlertType.TempOffMarketBackOnMarketDaysOrLess) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            listingSale.MlsStatus == MarketStatuses.Withdrawn &&
                            listingSale.StatusFieldsInfo.OffMarketDate.HasValue &&
                            listingSale.StatusFieldsInfo.BackOnMarketDate.HasValue &&
                            listingSale.StatusFieldsInfo.BackOnMarketDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) &&
                            listingSale.StatusFieldsInfo.BackOnMarketDate.Value > DateTime.UtcNow ? 1 : 0) : 0,
                TempOffMarketBackOnMarket = alerts.Contains(AlertType.TempOffMarketBackOnMarket) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            listingSale.MlsStatus == MarketStatuses.Withdrawn &&
                            listingSale.StatusFieldsInfo.OffMarketDate.HasValue &&
                            listingSale.StatusFieldsInfo.BackOnMarketDate.HasValue &&
                            listingSale.StatusFieldsInfo.BackOnMarketDate.Value <= DateTime.UtcNow ? 1 : 0) : 0,
                ActiveAndPendingListings = alerts.Contains(AlertType.ActiveAndPendingListing) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) ? 1 : 0) : 0,
                ComparableAndRelistListings = alerts.Contains(AlertType.ComparableAndRelistListing) ?
                        groupedListings.Sum(listingSale =>
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            Listing.RelistAndComparable.Contains(listingSale.PublishInfo.PublishType.Value) &&
                            listingSale.PublishInfo != null &&
                            listingSale.PublishInfo.PublishDate > FirstDayCurrentMonth ? 1 : 0) : 0,
                ExpiringListings = isMlsAdministrator && alerts.Contains(AlertType.ExpiringListings) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.ExpirationDate.HasValue &&
                            listingSale.ExpirationDate.Value > DateTime.UtcNow.AddYears(SaleListing.YearsInThePast) &&
                            listingSale.ExpirationDate.Value < DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysInTheFuture) ? 1 : 0) : 0,
                PastDueEstimatedClosingDate = alerts.Contains(AlertType.PastDueEstimatedClosingDate) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.PendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.StatusFieldsInfo.EstimatedClosedDate.HasValue &&
                            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value <= DateTime.UtcNow ? 1 : 0) : 0,
                EstimatedClosingDaysOrLess = alerts.Contains(AlertType.EstimatedClosingDaysOrLess) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.PendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.StatusFieldsInfo.EstimatedClosedDate.HasValue &&
                            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value > DateTime.UtcNow &&
                            listingSale.StatusFieldsInfo.EstimatedClosedDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) ? 1 : 0) : 0,
                PastDueEstimatedCompletionDate = alerts.Contains(AlertType.PastDueEstimatedCompletionDate) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Incomplete &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow ? 1 : 0) : 0,
                CompletionDateDueDaysOrLess = alerts.Contains(AlertType.CompletionDateDueDaysOrLess) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Incomplete &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value > DateTime.UtcNow &&
                            listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) ? 1 : 0) : 0,
                AgentBonusExpirationDate = alerts.Contains(AlertType.AgentBonusExpirationDate) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.SaleProperty.FinancialInfo.HasBonusWithAmount &&
                            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate <= DateTime.UtcNow ? 1 : 0) : 0,
                LockedListings = alerts.Contains(AlertType.LockedListings) ?
                    groupedListings.Sum(listingSale =>
                        Listing.LockedListingStatuses.Contains(listingSale.LockedStatus) ? 1 : 0) : 0,
                NotListedInMls = alerts.Contains(AlertType.NotListedInMls) ?
                    groupedListings.Sum(listingSale =>
                        string.IsNullOrEmpty(listingSale.MlsNumber) && listingSale.MlsStatus == MarketStatuses.Active ? 1 : 0) : 0,
                AgentBonusExpirationDateOrLess = alerts.Contains(AlertType.AgentBonusExpirationDateOrLess) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveAndPendingListingStatuses.Contains(listingSale.MlsStatus) &&
                            listingSale.SaleProperty.FinancialInfo.HasBonusWithAmount &&
                            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate > DateTime.UtcNow &&
                            listingSale.SaleProperty.FinancialInfo.BonusExpirationDate <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) ? 1 : 0) : 0,
                CurrentDaysOnMarketOverDays = alerts.Contains(AlertType.CurrentDaysOnMarketOverDays) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            listingSale.MlsStatus == MarketStatuses.Active &&
                            listingSale.MarketModifiedOn != null &&
                            listingSale.DOM != null &&
                            (listingSale.DOM.Value + EF.Functions.DateDiffDay(listingSale.MarketModifiedOn.Value, DateTime.UtcNow)) > SaleListing.MaxDaysInMarket ? 1 : 0) : 0,
                InadequatePublicRemarks = alerts.Contains(AlertType.InadequatePublicRemarks) ?
                        groupedListings.Sum(listingSale =>
                            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
                            SaleListing.ActiveListingStatuses.Contains(listingSale.MlsStatus) &&
                            !string.IsNullOrEmpty(listingSale.SaleProperty.FeaturesInfo.PropertyDescription) &&
                            listingSale.SaleProperty.FeaturesInfo.PropertyDescription.Length < SaleListing.MinPropertyDescriptionLength ? 1 : 0) : 0,
                OrphanListings = isMlsAdministrator && alerts.Contains(AlertType.OrphanListings) ?
                        groupedListings.Sum(listingSale =>
                            listingSale.SaleProperty.Community == null &&
                            SaleListing.OrphanListingStatuses.Contains(listingSale.MlsStatus) ? 1 : 0) : 0,
            }).SingleOrDefault();
    }
}
