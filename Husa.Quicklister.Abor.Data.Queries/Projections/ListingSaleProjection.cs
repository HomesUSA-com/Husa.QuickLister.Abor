namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class ListingSaleProjection
    {
        public static Expression<Func<SaleListing, ListingSaleQueryResult>> ProjectToListingSaleQueryResult => listingSale => new ListingSaleQueryResult
        {
            Id = listingSale.Id,
            MlsNumber = listingSale.MlsNumber,
            StreetName = listingSale.SaleProperty.AddressInfo.StreetName,
            StreetNum = listingSale.SaleProperty.AddressInfo.StreetNumber,
            City = listingSale.SaleProperty.AddressInfo.City,
            County = listingSale.SaleProperty.AddressInfo.County,
            ListDate = listingSale.ListDate,
            ListPrice = listingSale.ListPrice,
            MarketModifiedOn = listingSale.MarketModifiedOn,
            MlsStatus = listingSale.MlsStatus,
            SysModifiedOn = listingSale.SysModifiedOn,
            SysModifiedBy = listingSale.SysModifiedBy,
            Subdivision = listingSale.SaleProperty.AddressInfo.Subdivision,
            ZipCode = listingSale.SaleProperty.AddressInfo.ZipCode,
            OwnerName = listingSale.SaleProperty.OwnerName,
            MarketCode = MarketCode.Austin,
            SysCreatedOn = listingSale.SysCreatedOn,
            SysCreatedBy = listingSale.SysCreatedBy,
            CommunityId = listingSale.SaleProperty.CommunityId.Value,
            IsCompleteHome = listingSale.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete,
            Directions = listingSale.SaleProperty.ShowingInfo.Directions,
            PlanName = listingSale.SaleProperty.Plan != null ? listingSale.SaleProperty.Plan.BasePlan.Name : null,
            UnitNumber = listingSale.SaleProperty.AddressInfo.UnitNumber,
        };

        public static Expression<Func<SaleListing, ListingSaleQueryDetailResult>> ProjectToListingSaleQueryDetail => listingSale => new ListingSaleQueryDetailResult
        {
            Id = listingSale.Id,
            ExpirationDate = listingSale.ExpirationDate,
            ListDate = listingSale.ListDate,
            ListPrice = listingSale.ListPrice,
            ListType = listingSale.ListType,
            MlsNumber = listingSale.MlsNumber,
            MlsStatus = listingSale.MlsStatus,
            LockedBy = listingSale.LockedBy,
            LockedOn = listingSale.LockedOn,
            LockedStatus = listingSale.LockedStatus,
            SysCreatedBy = listingSale.SysCreatedBy,
            SysCreatedOn = listingSale.SysCreatedOn,
            SysModifiedBy = listingSale.SysModifiedBy,
            SysModifiedOn = listingSale.SysModifiedOn,
            IsPhotosDeclined = listingSale.IsPhotosDeclined,
            IsManuallyManaged = listingSale.IsManuallyManaged,
            StatusFieldsInfo = listingSale.StatusFieldsInfo.ToProjectionStatusFieldsInfo(),
            SaleProperty = listingSale.SaleProperty.ToProjection(),
            PublishInfo = listingSale.PublishInfo.ToProjectionPublishInfo(),
            EmailLead = listingSale.SaleProperty.Community.EmailLead.ToProjectionEmailLead(),
            XmlListingId = listingSale.XmlListingId,
            Directions = listingSale.SaleProperty.ShowingInfo.Directions,
            OwnerName = listingSale.SaleProperty.OwnerName,
            PlanName = listingSale.SaleProperty.Plan != null ? listingSale.SaleProperty.Plan.BasePlan.Name : null,
        };

        public static Expression<Func<SaleListing, ReversePorspectListingQueryResult>> ReverseProspectListingSaleQueryDetail => listingSale => new ReversePorspectListingQueryResult
        {
            ListingID = listingSale.Id,
            City = listingSale.SaleProperty.AddressInfo.City,
            CompanyId = listingSale.SaleProperty.CompanyId,
            MlsStatus = listingSale.MlsStatus,
            MarketCode = MarketCode.Austin,
            State = listingSale.SaleProperty.AddressInfo.State,
            StreetName = listingSale.SaleProperty.AddressInfo.StreetName,
            ZipCode = listingSale.SaleProperty.AddressInfo.ZipCode,
            StreetNumber = listingSale.SaleProperty.AddressInfo.StreetNumber,
            MlsNumber = listingSale.MlsNumber,
        };

        public static Expression<Func<SaleListing, ListingSaleBillingQueryResult>> ProjectToListingSaleBillingQueryResult => listingSale => new ListingSaleBillingQueryResult
        {
            Id = listingSale.Id,
            MlsNumber = listingSale.MlsNumber,
            StreetName = listingSale.SaleProperty.AddressInfo.StreetName,
            StreetNum = listingSale.SaleProperty.AddressInfo.StreetNumber,
            ListDate = listingSale.ListDate,
            MlsStatus = listingSale.MlsStatus,
            SysModifiedOn = listingSale.SysModifiedOn,
            SysModifiedBy = listingSale.SysModifiedBy,
            Subdivision = listingSale.SaleProperty.AddressInfo.Subdivision,
            ZipCode = listingSale.SaleProperty.AddressInfo.ZipCode,
            OwnerName = listingSale.SaleProperty.OwnerName,
            SysCreatedOn = listingSale.SysCreatedOn,
            SysCreatedBy = listingSale.SysCreatedBy,
            PublishDate = listingSale.PublishInfo.PublishDate,
            PublishStatus = listingSale.PublishInfo.PublishStatus,
            PublishType = listingSale.PublishInfo.PublishType,
            PublishUser = listingSale.PublishInfo.PublishUser,
        };

        public static Expression<Func<SaleListing, SaleListingOpenHouseQueryResult>> ProjectToSaleListingOpenHouseQueryResult => listingSale => new SaleListingOpenHouseQueryResult
        {
            CompanyId = listingSale.CompanyId,
            Id = listingSale.Id,
            MlsNumber = listingSale.MlsNumber,
            OpenHouses = listingSale.SaleProperty.OpenHouses.ToProjectionOpenHouses(),
        };
    }
}
