namespace Husa.Quicklister.Abor.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public static class ValidateListingProperty<TPropertyFields>
        where TPropertyFields : IPropertyInfo
    {
        public static ValidationResult GetErrors(MarketStatuses mlsStatus, TPropertyFields value)
        {
            var errors = GetValidations(mlsStatus, value).ToList();
            var validationContext = new ValidationContext(value, serviceProvider: null, items: null);
            Validator.TryValidateObject(value, validationContext, errors, validateAllProperties: true);

            if (errors.Count > 0)
            {
                CompositeValidationResult compositeValidationResult = new CompositeValidationResult($"Validation for PropertyFields failed!");
                errors.ToList().ForEach(compositeValidationResult.AddResult);
                return compositeValidationResult;
            }

            return ValidationResult.Success;
        }

        private static List<ValidationResult> GetValidations(MarketStatuses mlsStatus, TPropertyFields value)
        {
            switch (mlsStatus)
            {
                case MarketStatuses.Closed:
                    return SoldValidations(value);
            }

            return new List<ValidationResult>();
        }

        private static List<ValidationResult> SoldValidations(TPropertyFields record)
        {
            var results = new List<ValidationResult>();

            if (record.ConstructionStage.HasValue && record.ConstructionStage.Value != ConstructionStage.Complete)
            {
                results.Add(new("This field should be complete", new[] { nameof(record.ConstructionStage) }));
            }

            if (record.ConstructionCompletionDate.HasValue && record.ConstructionCompletionDate.Value.Date > DateTime.UtcNow.Date)
            {
                results.Add(new ValidationResult(OperatorType.LessEqual.GetErrorMessage("today"), new[] { nameof(record.ConstructionCompletionDate) }));
            }

            return results;
        }
    }
}
