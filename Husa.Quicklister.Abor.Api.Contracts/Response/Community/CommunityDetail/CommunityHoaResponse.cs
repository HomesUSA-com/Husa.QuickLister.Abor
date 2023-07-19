namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public class CommunityHoaResponse
    {
        public string Name { get; set; }
        public decimal TransferFee { get; set; }
        public decimal Fee { get; set; }
        public BillingFrequency BillingFrequency { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public string HoaType { get; set; }
    }
}
