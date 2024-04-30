namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class SaleAddressDto : AddressDto, IProvideSaleAddress
    {
        public string UnitNumber { get; set; }
    }
}
