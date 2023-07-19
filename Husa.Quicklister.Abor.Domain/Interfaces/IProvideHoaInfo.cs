namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IProvideHoaInfo
    {
        string Name { get; set; }

        decimal TransferFee { get; set; }

        decimal Fee { get; set; }

        BillingFrequency BillingFrequency { get; set; }

        string Website { get; set; }

        string ContactPhone { get; set; }
    }
}
