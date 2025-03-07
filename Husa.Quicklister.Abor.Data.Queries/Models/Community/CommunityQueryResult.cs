namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityQueryResult : BaseQueryResult
    {
        public string Name { get; set; }

        public string Builder { get; set; }

        public Cities? City { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public MarketCode Market { get; set; }
        public bool UseShowingTime { get; set; }
    }
}
