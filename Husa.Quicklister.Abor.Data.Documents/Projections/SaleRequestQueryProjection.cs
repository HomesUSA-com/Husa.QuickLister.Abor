namespace Husa.Quicklister.Abor.Data.Documents.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Data.Documents.Extensions;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class SaleRequestQueryProjection
    {
        public static Expression<Func<SaleListingRequest, ListingSaleRequestQueryResult>> ProjectionToRequestSale =>
            listingRequest => new ListingSaleRequestQueryResult()
            {
                Id = listingRequest.Id,
                OwnerName = listingRequest.SaleProperty.OwnerName,
                MlsNumber = listingRequest.MlsNumber,
                Market = "ABOR",
                MlsStatus = listingRequest.MlsStatus,
                City = listingRequest.SaleProperty.AddressInfo.City,
                Subdivision = listingRequest.SaleProperty.AddressInfo.Subdivision,
                ZipCode = listingRequest.SaleProperty.AddressInfo.ZipCode,
                Address = listingRequest.SaleProperty.Address,
                ListPrice = listingRequest.ListPrice,
                SysCreatedOn = listingRequest.SysCreatedOn,
                SysCreatedBy = listingRequest.SysCreatedBy,
                EnableOpenHouse = listingRequest.SaleProperty.ShowingInfo.EnableOpenHouses,
                UnitNumber = listingRequest.SaleProperty.AddressInfo.UnitNumber,
                UpdateGeocodes = listingRequest.SaleProperty.PropertyInfo.UpdateGeocodes,
                StreetType = listingRequest.SaleProperty.AddressInfo.StreetType,
            };

        public static Expression<Func<SaleListingRequest, ListingSaleRequestDetailQueryResult>> ProjectionToListingSaleRequestDetailQueryResult =>
            listingRequest => new ListingSaleRequestDetailQueryResult()
            {
                ListingSaleId = listingRequest.ListingSaleId,
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
                StatusFieldsInfo = listingRequest.StatusFieldsInfo.ToProjectionListingSaleRequestStatusFieldsQueryResult(),
                SaleProperty = listingRequest.SaleProperty.ToProjectionListingSaleRequestSalePropertyQueryResult(),
            };

        public static ListingSaleRequestDetailQueryResult ToListingSaleRequestDetailQueryResult(this SaleListingRequest listingRequest) =>
            new()
            {
                ListingSaleId = listingRequest.ListingSaleId,
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
                StatusFieldsInfo = listingRequest.StatusFieldsInfo.ToProjectionListingSaleRequestStatusFieldsQueryResult(),
                SaleProperty = listingRequest.SaleProperty.ToProjectionListingSaleRequestSalePropertyQueryResult(),
                PublishInfo = listingRequest.PublishInfo.ToProjectionListingRequestPublishInfoQueryResult(),
            };
    }
}
