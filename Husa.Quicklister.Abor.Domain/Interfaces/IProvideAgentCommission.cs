namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface IProvideAgentCommission
    {
        decimal? BuyersAgentCommission { get; set; }
        CommissionType BuyersAgentCommissionType { get; set; }
    }
}
