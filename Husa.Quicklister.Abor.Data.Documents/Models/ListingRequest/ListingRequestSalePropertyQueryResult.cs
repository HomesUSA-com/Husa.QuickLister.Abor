namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;

    public class ListingRequestSalePropertyQueryResult
    {
        public virtual SalePropertyQueryResult SalePropertyInfo { get; set; }

        public virtual AddressInfoQueryResult AddressInfo { get; set; }

        public virtual PropertyInfoQueryResult PropertyInfo { get; set; }

        public virtual SpacesDimensionsInfoQueryResult SpacesDimensionsInfo { get; set; }

        public virtual FeaturesInfoQueryResult FeaturesInfo { get; set; }

        public virtual FinancialInfoQueryResult FinancialInfo { get; set; }

        public virtual ShowingInfoQueryResult ShowingInfo { get; set; }

        public virtual SchoolsInfoQueryResult SchoolsInfo { get; set; }

        public virtual IEnumerable<RoomQueryResult> Rooms { get; set; }

        public virtual IEnumerable<OpenHousesQueryResult> OpenHouses { get; set; }
    }
}
