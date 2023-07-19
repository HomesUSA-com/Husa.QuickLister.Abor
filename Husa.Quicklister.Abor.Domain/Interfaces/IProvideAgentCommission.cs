namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IProvideAgentCommission
    {
        decimal? BuyersAgentCommission { get; set; }
        CommissionType BuyersAgentCommissionType { get; set; }
    }
}
