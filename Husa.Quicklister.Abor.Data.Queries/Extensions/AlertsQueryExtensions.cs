namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;

    public static class AlertsQueryExtensions
    {
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
            { AlertType.CompletedHomesWithoutPhotoRequest, CompletedHomesWithoutPhotoRequestExpression },
            { AlertType.LockedListingsImported, LockedListingsImportedExpression },
        };

        // Temp Off Market - Back on Market (BOM) Date - Due in 7 days or Less
        public static Expression<Func<SaleListing, bool>> TempOffMarketBackOnMarketDaysOrLessExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.MlsStatus == MarketStatuses.Hold &&
            listingSale.StatusFieldsInfo.OffMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.HasValue &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.Value <= DateTime.UtcNow.AddDays(SaleListing.MaxExpirationDaysLeftInTheFuture) &&
            listingSale.StatusFieldsInfo.BackOnMarketDate.Value > DateTime.UtcNow;

        // Temp Off Market - Back on Market (BOM) Date - Past Due
        public static Expression<Func<SaleListing, bool>> TempOffMarketBackOnMarketExpression => listingSale =>
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.MlsStatus == MarketStatuses.Hold &&
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
            SaleListing.PendingAndActiveUnderContractStatuses.Contains(listingSale.MlsStatus) &&
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

        // Locked Listings Imported
        public static Expression<Func<SaleListing, bool>> LockedListingsImportedExpression => listingSale => Listing.LockedListingStatuses.Contains(listingSale.LockedStatus);

        // Locked Listings
        public static Expression<Func<SaleListing, bool>> LockedListingsExpression => listingSale =>
            Listing.LockedListingStatuses.Contains(listingSale.LockedStatus) &&
            !string.IsNullOrEmpty(listingSale.MlsNumber) &&
            listingSale.SysModifiedOn < DateTime.UtcNow.Date;

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
            (listingSale.SaleProperty.FeaturesInfo.PropertyDescription.Length < SaleListing.MinPropertyDescriptionLength ||
            EF.Functions.Like(listingSale.SaleProperty.FeaturesInfo.PropertyDescription, "%bonus%"));

        // Orphan Listings
        public static Expression<Func<SaleListing, bool>> OrphanListingsExpression => listingSale =>
            listingSale.SaleProperty.Community == null &&
            SaleListing.OrphanListingStatuses.Contains(listingSale.MlsStatus);

        // Completed Homes Without PhotoRequest
        public static Expression<Func<SaleListing, bool>> CompletedHomesWithoutPhotoRequestExpression => listing =>
            SaleListing.ActivePhotoRequestListingStatuses.Contains(listing.MlsStatus) &&
            !string.IsNullOrEmpty(listing.MlsNumber) && !listing.LastPhotoRequestId.HasValue &&
            listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
            listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow &&
            !listing.IsPhotosDeclined;
    }
}
