namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class ShowingValidator : AbstractValidator<ShowingRequest>
    {
        public ShowingValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(x => x.AgentPrivateRemarks).MaximumLength(1000);
            this.RuleFor(x => x.AgentPrivateRemarksAdditional).MaximumLength(1000);
            this.RuleFor(x => x.Directions).NotEmpty().MaximumLength(2000);
        }
    }
}
