namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class PropertyInfoValidator : AbstractValidator<PropertyInfoRequest>
    {
        public PropertyInfoValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(pi => pi.LotSize).NotEmpty().MaximumLength(25);
            this.RuleFor(pi => pi.LegalDescription).NotEmpty().MaximumLength(255);
            this.RuleFor(pi => pi.TaxLot).NotEmpty().MaximumLength(25);
            this.RuleFor(pi => pi.ConstructionCompletionDate).NotEmpty().GreaterThanOrEqualTo(DateTimeExtensions.TodayUtc().AddYears(-5));
        }
    }
}
