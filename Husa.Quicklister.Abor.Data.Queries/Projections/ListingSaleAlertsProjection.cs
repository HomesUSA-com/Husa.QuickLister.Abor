namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;

    public static class ListingSaleAlertsProjection
    {
        public static Expression<Func<SaleListing, DetailAlertQueryResult>> ProjectListingSaleQueryResult() => listingSale => new()
        {
            Id = listingSale.Id,
            MarketCode = MarketCode.SanAntonio,
            MlsNumber = listingSale.MlsNumber,
            MlsStatus = listingSale.MlsStatus,
            Address = listingSale.SaleProperty.Address,
            Subdivision = listingSale.SaleProperty.AddressInfo.Subdivision,
            OwnerName = listingSale.SaleProperty.OwnerName,
            SysModifiedBy = listingSale.SysModifiedBy,
            BonusExpirationDate = listingSale.SaleProperty.FinancialInfo.BonusExpirationDate,
            EstimatedClosedDate = listingSale.StatusFieldsInfo.EstimatedClosedDate,
            ConstructionCompletionDate = listingSale.SaleProperty.PropertyInfo.ConstructionCompletionDate,
            BackOnMarketDate = listingSale.StatusFieldsInfo.BackOnMarketDate,
            OffMarketDate = listingSale.StatusFieldsInfo.OffMarketDate,
            ExpirationDate = listingSale.ExpirationDate,
            LockedBy = listingSale.LockedBy,
            SysModifiedOn = listingSale.SysModifiedOn,
            PublicRemarks = listingSale.SaleProperty.FeaturesInfo.PropertyDescription,
            DOM = listingSale.DOM,
            SysCreatedBy = listingSale.SysCreatedBy,
            PublishDate = listingSale.PublishInfo.PublishDate,
            PublishStatus = listingSale.PublishInfo.PublishStatus,
            PublishType = listingSale.PublishInfo.PublishType,
            PublishUser = listingSale.PublishInfo.PublishUser,
            CommunityEmployees = ProjectToUserQueryResults(listingSale),
        };

        public static IEnumerable<UserQueryResult> ProjectToUserQueryResults(SaleListing listingSale) =>
            listingSale.SaleProperty.Community != null
                ? listingSale.SaleProperty.Community.Employees.Select(e => new UserQueryResult() { UserId = e.UserId })
                : Array.Empty<UserQueryResult>();
    }
}
