namespace Husa.Quicklister.Abor.Api.Contracts.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RequiredIfNullAttribute : ValidationAttribute
    {
        public RequiredIfNullAttribute(string dependentProperty)
        {
            this.DependentProperty = dependentProperty ?? throw new ArgumentNullException(nameof(dependentProperty), "dependentProperty cannot be null");
            this.ErrorMessage = "value is required";
        }

        public string DependentProperty { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this.DependentProperty);
            if (value is null && field != null)
            {
                var dependentCurrentValue = field.GetValue(validationContext.ObjectInstance, null);

                if (dependentCurrentValue is null)
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
