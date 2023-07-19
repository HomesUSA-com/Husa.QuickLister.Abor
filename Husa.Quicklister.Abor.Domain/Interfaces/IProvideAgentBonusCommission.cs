namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IProvideAgentBonusCommission
    {
        decimal? AgentBonusAmount { get; set; }

        CommissionType AgentBonusAmountType { get; set; }
    }
}
