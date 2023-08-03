namespace Husa.Quicklister.Abor.Api.ValidationsRules.SalePropertyValidations
{
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request;

    public class SchoolValidator : AbstractValidator<SchoolsRequest>
    {
        public SchoolValidator()
        {
            this.RuleFor(x => x.SchoolDistrict).NotEmpty();
            this.RuleFor(x => x.ElementarySchool).NotEmpty();
            this.RuleFor(x => x.MiddleSchool).NotEmpty();
            this.RuleFor(x => x.HighSchool).NotEmpty();
        }
    }
}
