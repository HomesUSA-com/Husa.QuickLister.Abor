namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class AddressQueryResult
    {
        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public Cities City { get; set; }

        public States State { get; set; }

        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public string LotNum { get; set; }

        public string Block { get; set; }

        public string Subdivision { get; set; }
    }
}
