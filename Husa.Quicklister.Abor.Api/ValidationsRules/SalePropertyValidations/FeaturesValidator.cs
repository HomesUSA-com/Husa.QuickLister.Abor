namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class FeaturesValidator : AbstractValidator<FeaturesRequest>
    {
        public FeaturesValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(sp => sp.PropertyDescription).NotEmpty().MaximumLength(4000);
            this.RuleFor(sp => sp.Accessibility).NotEmpty().When(x => x.HasAccessibility);
            this.RuleFor(sp => sp.FireplaceDescription).NotEmpty().When(x => x.Fireplaces > 0);
            this.RuleFor(sp => sp.HasPrivatePool).NotNull();
            this.RuleFor(sp => sp.PrivatePool).NotEmpty().When(x => x.HasPrivatePool);
            this.RuleFor(sp => sp.Inclusions).NotEmpty();
            this.RuleFor(sp => sp.Floors).NotEmpty();
            this.RuleFor(sp => sp.WindowCoverings).NotEmpty();
            this.RuleFor(sp => sp.Exterior).NotEmpty();
            this.RuleFor(sp => sp.HousingStyle).NotEmpty();
            this.RuleFor(sp => sp.RoofDescription).NotEmpty();
            this.RuleFor(sp => sp.Foundation).NotEmpty();
            this.RuleFor(sp => sp.HeatSystem).NotEmpty();
            this.RuleFor(sp => sp.HeatingFuel).NotEmpty();
            this.RuleFor(sp => sp.CoolingSystem).NotEmpty();
            this.RuleFor(sp => sp.NeighborhoodAmenities).NotEmpty();
        }
    }
}
