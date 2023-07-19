namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public class HoaResponse
    {
        public string Name { get; set; }
        public decimal TransferFee { get; set; }
        public decimal Fee { get; set; }
        public BillingFrequency BillingFrequency { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
    }
}
