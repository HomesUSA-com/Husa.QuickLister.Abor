namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;

    public class FinancialValidator : AbstractValidator<FinancialRequest>
    {
        public FinancialValidator()
        {
            this.RuleFor(x => x.BuyersAgentCommission)
                .LessThanOrEqualTo(40000)
                .GreaterThanOrEqualTo(1000)
                .When(x => x.BuyersAgentCommissionType == Quicklister.Extensions.Domain.Enums.CommissionType.Amount);
            this.RuleFor(x => x.BuyersAgentCommission)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(8)
                .When(x => x.BuyersAgentCommissionType == Quicklister.Extensions.Domain.Enums.CommissionType.Percent);
        }
    }
}
