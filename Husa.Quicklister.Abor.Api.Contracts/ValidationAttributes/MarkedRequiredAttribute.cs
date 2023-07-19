namespace Husa.Quicklister.Abor.Api.Contracts.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public sealed class MarkedRequiredAttribute : RequiredAttribute
    {
        public MarkedRequiredAttribute()
            : base()
        {
            this.AllowEmptyStrings = false;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fieldValue = value is not null ? value.ToString() : string.Empty;

            if (string.IsNullOrEmpty(fieldValue))
            {
                this.ErrorMessage = "value is required.";
            }

            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
