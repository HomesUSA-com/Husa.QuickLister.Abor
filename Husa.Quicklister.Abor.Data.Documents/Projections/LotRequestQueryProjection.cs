namespace Husa.Quicklister.Abor.Data.Documents.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Data.Documents.Extensions;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.LotRequest;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Extensions.Lot;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class LotRequestQueryProjection
    {
        public static Expression<Func<LotListingRequest, ListingRequestQueryResult>> ProjectionToLotRequest =>
            listingRequest => new ListingRequestQueryResult()
            {
                Id = listingRequest.Id,
                OwnerName = listingRequest.OwnerName,
                MlsNumber = listingRequest.MlsNumber,
                Market = "ABOR",
                MlsStatus = listingRequest.MlsStatus,
                City = listingRequest.AddressInfo.City,
                Subdivision = listingRequest.AddressInfo.Subdivision,
                ZipCode = listingRequest.AddressInfo.ZipCode,
                ListPrice = listingRequest.ListPrice ?? 0,
                SysCreatedOn = listingRequest.SysCreatedOn,
                SysCreatedBy = listingRequest.SysCreatedBy,
                UnitNumber = listingRequest.AddressInfo.UnitNumber,
                UpdateGeocodes = listingRequest.PropertyInfo.UpdateGeocodes,
                StreetType = listingRequest.AddressInfo.StreetType,
                Address = $"{listingRequest.AddressInfo.StreetNumber} {listingRequest.AddressInfo.StreetName}",
            };

        public static Expression<Func<LotListingRequest, LotListingRequestDetailQueryResult>> ProjectionToLotListingRequestDetailQueryResult =>
            listingRequest => new LotListingRequestDetailQueryResult()
            {
                ListingId = listingRequest.EntityId,
                ExpirationDate = listingRequest.ExpirationDate,
                ListDate = listingRequest.ListDate,
                ListPrice = listingRequest.ListPrice,
                MlsNumber = listingRequest.MlsNumber,
                MlsStatus = listingRequest.MlsStatus,
                RequestState = listingRequest.RequestState,
                CommunityId = listingRequest.CommunityId,
                CompanyId = listingRequest.CompanyId,
                OwnerName = listingRequest.OwnerName,
                Id = listingRequest.Id,
                LockedOn = null,
                LockedByUsername = null,
                LockedBy = null,
                LockedStatus = LockedStatus.NoLocked,
                StatusFieldsInfo = listingRequest.StatusFieldsInfo.ToProjectionStatusFieldsQueryResult(),
            };

        public static LotListingRequestDetailQueryResult ToLotListingRequestDetailQueryResult(this LotListingRequest listingRequest) =>
            new()
            {
                ListingId = listingRequest.EntityId,
                CommunityId = listingRequest.CommunityId,
                CompanyId = listingRequest.CompanyId,
                OwnerName = listingRequest.OwnerName,
                ExpirationDate = listingRequest.ExpirationDate,
                ListDate = listingRequest.ListDate,
                ListPrice = listingRequest.ListPrice,
                MlsNumber = listingRequest.MlsNumber,
                MlsStatus = listingRequest.MlsStatus,
                RequestState = listingRequest.RequestState,
                Id = listingRequest.Id,
                LockedOn = null,
                LockedByUsername = null,
                LockedBy = null,
                LockedStatus = LockedStatus.NoLocked,
                SysCreatedOn = listingRequest.SysCreatedOn,
                SysCreatedBy = listingRequest.SysCreatedBy,
                SysModifiedOn = listingRequest.SysModifiedOn,
                SysModifiedBy = listingRequest.SysModifiedBy,
                StatusFieldsInfo = listingRequest.StatusFieldsInfo.ToProjectionStatusFieldsQueryResult(),
                PublishInfo = listingRequest.PublishInfo.ToProjectionPublishInfo(),
                AddressInfo = listingRequest.AddressInfo.ToProjectionAddressInfo(),
                SchoolsInfo = listingRequest.SchoolsInfo.ToProjectionSchools(),
                FeaturesInfo = listingRequest.FeaturesInfo.ToProjectionFeatures(),
                PropertyInfo = listingRequest.PropertyInfo.ToProjectionPropertyInfo(),
                FinancialInfo = listingRequest.FinancialInfo.ToProjectionFinancial(),
                ShowingInfo = listingRequest.ShowingInfo.ToProjectionShowing(),
            };

        private static ListingStatusFieldsQueryResult ToProjectionStatusFieldsQueryResult(this LotStatusFieldsRecord statusField)
            => statusField.ToProjectionStatusFieldsQueryResult<LotStatusFieldsRecord, ListingStatusFieldsQueryResult>();
    }
}
