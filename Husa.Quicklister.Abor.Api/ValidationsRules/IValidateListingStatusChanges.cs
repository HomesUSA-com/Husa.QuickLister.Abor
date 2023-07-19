namespace Husa.Quicklister.Abor.Api.ValidationsRules
{
    using FluentValidation.Results;

    public interface IValidateListingStatusChanges<in T>
    {
        ValidationResult Validate(T instance);
    }
}
