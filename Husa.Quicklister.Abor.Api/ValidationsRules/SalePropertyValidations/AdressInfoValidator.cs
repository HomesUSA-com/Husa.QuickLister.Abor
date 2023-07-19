namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class AdressInfoValidator : AbstractValidator<AddressInfoRequest>
    {
        public AdressInfoValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(bp => bp.LotNum).NotEmpty().MaximumLength(5);
            this.RuleFor(bp => bp.ZipCode).NotEmpty().MaximumLength(5).MinimumLength(5);
            this.RuleFor(bp => bp.Block).NotEmpty().MaximumLength(5);
            this.RuleFor(bp => bp.Subdivision).NotEmpty().MaximumLength(75);
        }
    }
}
