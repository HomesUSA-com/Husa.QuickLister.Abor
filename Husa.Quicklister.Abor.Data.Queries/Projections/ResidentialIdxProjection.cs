namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using AddressQueryResult = Husa.Quicklister.Extensions.Data.Queries.Models.AddressQueryResult;

    public static class ResidentialIdxProjection
    {
        public static Expression<Func<SaleListing, ResidentialIdxQueryResult>> ToResidentialIdxQueryResult => listing => new ResidentialIdxQueryResult
        {
            Id = listing.Id,
            XmlId = listing.XmlListingId,
            MlsNumber = listing.MlsNumber,
            ListDate = listing.ListDate,
            ListPrice = listing.ListPrice,
            MlsStatus = listing.MlsStatus.ToString(),
            MarketCode = MarketCode.Austin,
            Address = listing.SaleProperty.AddressInfo.ToAddressQueryResult(),
        };

        private static AddressQueryResult ToAddressQueryResult(this SaleAddressInfo addressInfo)
        {
            if (addressInfo == null)
            {
                return new();
            }

            return new()
            {
                StreetNumber = addressInfo.StreetNumber,
                StreetName = addressInfo.StreetName,
                City = addressInfo.City.ToString(),
                State = addressInfo.State,
                ZipCode = addressInfo.ZipCode,
                County = addressInfo.County.ToString(),
                StreetType = addressInfo.StreetType.ToString(),
                Subdivision = addressInfo.Subdivision,
                UnitNumber = addressInfo.UnitNumber,
            };
        }
    }
}
