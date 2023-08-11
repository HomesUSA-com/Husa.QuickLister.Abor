namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class FinancialValidator : AbstractValidator<FinancialRequest>
    {
        public FinancialValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(x => x.TitleCompany).NotEmpty().MaximumLength(45);
            this.RuleFor(x => x.HOARequirement).NotNull();
        }
    }
}
