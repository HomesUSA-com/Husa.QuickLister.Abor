namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Data.Queries.Extensions;
    using Husa.Quicklister.Extensions.Data.Queries.Models;

    public static class BillingProjection
    {
        public static Expression<Func<SaleListing, SaleListingBillingQueryResult>> ProjectToListingSaleBillingQueryResult => listing => new()
        {
            Id = listing.Id,
            MlsNumber = listing.MlsNumber,
            StreetName = listing.SaleProperty.AddressInfo.StreetName,
            StreetNum = listing.SaleProperty.AddressInfo.StreetNumber,
            ListDate = listing.ListDate,
            MlsStatus = listing.MlsStatus.ToCamelCase(),
            SysModifiedOn = listing.SysModifiedOn,
            SysModifiedBy = listing.SysModifiedBy,
            Subdivision = listing.SaleProperty.AddressInfo.Subdivision,
            ZipCode = listing.SaleProperty.AddressInfo.ZipCode,
            OwnerName = listing.SaleProperty.OwnerName,
            SysCreatedOn = listing.SysCreatedOn,
            SysCreatedBy = listing.SysCreatedBy,
            PublishDate = listing.PublishInfo.PublishDate,
            PublishStatus = listing.PublishInfo.PublishStatus.ToCamelCase(),
            PublishType = listing.PublishInfo.PublishType,
            PublishUser = listing.PublishInfo.PublishUser,
            CompanyId = listing.CompanyId,
        };
    }
}
