namespace Husa.Quicklister.Abor.Data.Queries.Projections.IdxProjection
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions.Sale;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;

    public static class ResidentialIdxProjection
    {
        public static Expression<Func<SaleListing, ResidentialIdxQueryResult>> ToResidentialIdxQueryResult => listing => new ResidentialIdxQueryResult
        {
            Id = listing.Id,
            XmlId = listing.XmlListingId,
            MlsNumber = listing.MlsNumber,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            MlsStatus = listing.MlsStatus,
            MarketCode = MarketCode.Austin,
            Address = listing.SaleProperty.AddressInfo.ToProjectionSaleAddressInfo(),
        };
    }
}
