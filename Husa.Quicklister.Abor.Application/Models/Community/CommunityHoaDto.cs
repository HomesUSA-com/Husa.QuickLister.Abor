namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public class CommunityHoaDto
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
