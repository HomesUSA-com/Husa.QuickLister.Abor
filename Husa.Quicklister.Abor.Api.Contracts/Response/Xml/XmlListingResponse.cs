namespace Husa.Quicklister.Abor.Api.Contracts.Response.Xml
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class XmlListingResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }

        public string Builder { get; set; }

        public string Subdivision { get; set; }

        public MarketCode Market { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }

        public int? Status { get; set; }

        public Cities City { get; set; }

        public Counties? County { get; set; }

        public States? State { get; set; }

        public string Zip { get; set; }

        public string Street2 { get; set; }

        public string StreetName { get; set; }

        public string Country { get; set; }

        public string Street1 { get; set; }

        public string Phone { get; set; }

        public DateTime? ObtainedAt { get; set; }

        public DateTime? ListLaterDate { get; set; }
    }
}
