namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface IProvideAgentBonusCommission
    {
        decimal? AgentBonusAmount { get; set; }

        CommissionType AgentBonusAmountType { get; set; }
    }
}
