namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Extensions.Lot;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Extensions.Data.Queries.Projections;

    public static class LotListingProjection
    {
        public static Expression<Func<LotListing, LotListingQueryResult>> ProjectToLotListingQueryResult => listing => new LotListingQueryResult
        {
            Id = listing.Id,
            MlsNumber = listing.MlsNumber,
            StreetName = listing.AddressInfo.StreetName,
            StreetNum = listing.AddressInfo.StreetNumber,
            StreetType = listing.AddressInfo.StreetType,
            State = listing.AddressInfo.State,
            City = listing.AddressInfo.City,
            County = listing.AddressInfo.County,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            MarketModifiedOn = listing.MarketModifiedOn,
            MlsStatus = listing.MlsStatus,
            SysModifiedOn = listing.SysModifiedOn,
            SysModifiedBy = listing.SysModifiedBy,
            Subdivision = listing.AddressInfo.Subdivision,
            ZipCode = listing.AddressInfo.ZipCode,
            OwnerName = listing.OwnerName,
            MarketCode = MarketCode.Austin,
            SysCreatedOn = listing.SysCreatedOn,
            SysCreatedBy = listing.SysCreatedBy,
            CommunityId = listing.CommunityId,
            UnitNumber = listing.AddressInfo.UnitNumber,
        };

        public static Expression<Func<LotListing, LotListingQueryDetailResult>> ProjectToLotListingQueryDetail => listing => new LotListingQueryDetailResult
        {
            Id = listing.Id,
            ExpirationDate = listing.ExpirationDate,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            ListType = listing.ListType,
            MlsNumber = listing.MlsNumber,
            MlsStatus = listing.MlsStatus,
            LockedBy = listing.LockedBy,
            LockedOn = listing.LockedOn,
            LockedStatus = listing.LockedStatus,
            SysCreatedBy = listing.SysCreatedBy,
            SysCreatedOn = listing.SysCreatedOn,
            SysModifiedBy = listing.SysModifiedBy,
            SysModifiedOn = listing.SysModifiedOn,
            IsPhotosDeclined = listing.IsPhotosDeclined,
            IsManuallyManaged = listing.IsManuallyManaged,
            XmlListingId = listing.XmlListingId,
            XmlDiscrepancyListingId = listing.XmlDiscrepancyListingId,
            OwnerName = listing.OwnerName,
            CommunityId = listing.CommunityId,
            CompanyId = listing.CompanyId,
            AddressInfo = listing.AddressInfo.ToProjectionAddressInfo(),
            SchoolsInfo = listing.SchoolsInfo.ToProjectionSchools(),
            FeaturesInfo = listing.FeaturesInfo.ToProjectionFeatures(),
            PropertyInfo = listing.PropertyInfo.ToProjectionPropertyInfo(),
            FinancialInfo = listing.FinancialInfo.ToProjectionFinancial(),
            ShowingInfo = listing.ShowingInfo.ToProjectionShowing(),
            PublishInfo = listing.PublishInfo.ToProjectionPublishInfo(),
            EmailLead = listing.Community.EmailLead.ToProjectionEmailLead(),
            StatusFieldsInfo = listing.StatusFieldsInfo.ToProjectionStatusFieldsInfo<ListingStatusFieldsInfo, ListingStatusFieldsQueryResult>(),
        };
    }
}
