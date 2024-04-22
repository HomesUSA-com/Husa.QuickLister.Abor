namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class AddressDto : IProvideAddress
    {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public Cities City { get; set; }
        public States State { get; set; }
        public string ZipCode { get; set; }
        public Counties? County { get; set; }
        public StreetType? StreetType { get; set; }
        public string UnitNumber { get; set; }
        public string Subdivision { get; set; }

        public string Address => $"{this.StreetNumber} {this.StreetName}";
    }
}
