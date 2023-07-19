namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System.Collections.Generic;
    using Husa.Quicklister.Extensions.Data.Queries.Models;

    public class SalePropertyDetailQueryResult
    {
        public SalePropertyQueryResult SalePropertyInfo { get; set; }

        public PropertyInfoQueryResult PropertyInfo { get; set; }

        public AddressQueryResult AddressInfo { get; set; }

        public SpacesDimensionsQueryResult SpacesDimensionsInfo { get; set; }

        public FeaturesQueryResult FeaturesInfo { get; set; }

        public FinancialQueryResult FinancialInfo { get; set; }

        public ListingShowingQueryResult ShowingInfo { get; set; }

        public SchoolsInfoQueryResult SchoolsInfo { get; set; }

        public ICollection<RoomQueryResult> Rooms { get; set; }

        public ICollection<HoaQueryResult> Hoas { get; set; }

        public ICollection<OpenHousesQueryResult> OpenHouses { get; set; }
    }
}
