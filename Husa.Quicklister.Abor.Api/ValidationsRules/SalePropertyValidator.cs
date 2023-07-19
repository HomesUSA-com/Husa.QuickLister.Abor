namespace Husa.Quicklister.Abor.Api.ValidationsRules
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Api.ValidationsRules.Extensions;
    using Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class SalePropertyValidator : AbstractValidator<SalePropertyDetailRequest>
    {
        public SalePropertyValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(sp => sp.AddressInfo).SetValidator(new AdressInfoValidator());
            this.RuleFor(sp => sp.PropertyInfo).SetValidator(new PropertyInfoValidator());
            this.RuleFor(sp => sp.SpacesDimensionsInfo).SetValidator(new SpacesDimensionsValidator());
            this.RuleFor(sp => sp.FeaturesInfo).SetValidator(new FeaturesValidator());
            this.RuleFor(sp => sp.SchoolsInfo).SetValidator(new SchoolValidator());
            this.RuleFor(sp => sp.FinancialInfo).SetValidator(new FinancialValidator());
            this.RuleFor(sp => sp.ShowingInfo).SetValidator(new ShowingValidator());
            this.RuleFor(sp => sp.Rooms).ValidateListCount(1);
            this.RuleFor(sp => sp.Hoas).NotEmpty().When(sp => sp.FinancialInfo.HOARequirement == HoaRequirement.Mandatory);
        }
    }
}
