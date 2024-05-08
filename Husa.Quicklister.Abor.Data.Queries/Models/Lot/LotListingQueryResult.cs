namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotListingQueryResult : ListingQueryResult
    {
        public string StreetNum { get; set; }

        public string StreetName { get; set; }

        public StreetType? StreetType { get; set; }

        public Cities City { get; set; }

        public Counties? County { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public MarketCode MarketCode { get; set; }

        public States State { get; set; }

        public Guid? CommunityId { get; set; }
    }
}
