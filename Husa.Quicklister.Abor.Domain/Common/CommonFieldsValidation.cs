namespace Husa.Quicklister.Abor.Domain.Common
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public static class CommonFieldsValidation
    {
        public static bool IsValidBuyersAgentCommissionRange(this IProvideAgentCommission agentCommission)
        {
            if (agentCommission.BuyersAgentCommission == null)
            {
                return true;
            }

            if (agentCommission.BuyersAgentCommission < 1)
            {
                return false;
            }

            return agentCommission.BuyersAgentCommissionType switch
            {
                CommissionType.Percent => agentCommission.BuyersAgentCommission <= 8,
                CommissionType.Amount => agentCommission.BuyersAgentCommission <= 50000,
                _ => false,
            };
        }

        public static bool IsValidAgentBonusAmountRange(this IProvideAgentBonusCommission agentBonusCommision)
        {
            if (agentBonusCommision.AgentBonusAmount == null)
            {
                return true;
            }

            if (agentBonusCommision.AgentBonusAmount < 1)
            {
                return false;
            }

            return agentBonusCommision.AgentBonusAmountType switch
            {
                CommissionType.Percent => agentBonusCommision.AgentBonusAmount <= 8,
                CommissionType.Amount => agentBonusCommision.AgentBonusAmount <= 30000,
                _ => false,
            };
        }
    }
}
