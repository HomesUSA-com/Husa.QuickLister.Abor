namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;

    public class SpacesDimensionsValidator : AbstractValidator<SpacesDimensionsRequest>
    {
        public SpacesDimensionsValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;
            this.RuleFor(sps => sps.FullBathsTotal).NotNull();
            this.RuleFor(sps => sps.HalfBathsTotal).NotNull();
            this.RuleFor(sps => sps.StoriesTotal).IsInEnum();
            this.RuleFor(sps => sps.DiningAreasTotal).NotNull();
            this.RuleFor(sps => sps.LivingAreasTotal).NotNull();
            this.RuleFor(sps => sps.TypeCategory).IsInEnum();
            this.RuleFor(sp => sp.SpecialtyRooms).NotEmpty();
            this.RuleFor(sps => sps.SqFtTotal).LessThanOrEqualTo(10000).GreaterThanOrEqualTo(500);
        }
    }
}
