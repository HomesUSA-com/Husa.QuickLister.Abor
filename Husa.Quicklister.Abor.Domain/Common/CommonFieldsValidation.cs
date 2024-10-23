namespace Husa.Quicklister.Abor.Domain.Common
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public static class CommonFieldsValidation
    {
        public const double MaxBuyersAgentAmount = 40000;
        public const double MaxAgentBonusAmount = 30000;
        public const double MaxCommissionPercent = 8;

        public static bool IsValidHoa(this IProvideCommonFinancial financialInfo)
        {
            return !financialInfo.HasHoa || financialInfo.HOARequirement.HasValue;
        }

        public static bool IsValidStage(MarketStatuses mlsStatus, ConstructionStage constructionStage)
        {
            return mlsStatus != MarketStatuses.Closed || constructionStage == ConstructionStage.Complete;
        }

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
                CommissionType.Percent => agentCommission.BuyersAgentCommission <= (decimal)MaxCommissionPercent,
                CommissionType.Amount => agentCommission.BuyersAgentCommission <= (decimal)MaxBuyersAgentAmount,
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
                CommissionType.Percent => agentBonusCommision.AgentBonusAmount <= (decimal)MaxCommissionPercent,
                CommissionType.Amount => agentBonusCommision.AgentBonusAmount <= (decimal)MaxAgentBonusAmount,
                _ => false,
            };
        }
    }
}
