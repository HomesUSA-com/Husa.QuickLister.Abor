namespace Husa.Quicklister.Abor.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Common;

    public static class ValidateListingStatus<TStatusFields>
        where TStatusFields : IProvideStatusFields
    {
        private const string RequiredFieldMessage = "This field is required";
        private const string GreaterThanErrorMessage = "This field must be greater than";
        private const string GreaterThanEqualToErrorMessage = "This field must be greater or equal to";
        private const string LessThanErrorMessage = "This field must be less than";
        private const string LessThanOrEqualToErrorMessage = "This field must be less or equal to";

        public static ValidationResult GetErrors(MarketStatuses mlsStatus, TStatusFields value)
        {
            var errors = GetValidations(mlsStatus, value).ToList();
            var validationContext = new ValidationContext(value, serviceProvider: null, items: null);
            Validator.TryValidateObject(value, validationContext, errors, validateAllProperties: true);

            if (errors.Any())
            {
                CompositeValidationResult compositeValidationResult = new CompositeValidationResult($"Validation for StatusFields failed!");
                errors.ToList().ForEach(compositeValidationResult.AddResult);
                return compositeValidationResult;
            }

            return ValidationResult.Success;
        }

        private static IEnumerable<ValidationResult> GetValidations(MarketStatuses mlsStatus, TStatusFields value)
        {
            switch (mlsStatus)
            {
                case MarketStatuses.Pending:
                    return PendingValidations(value);
            }

            return new List<ValidationResult>();
        }

        private static IEnumerable<ValidationResult> PendingValidations(TStatusFields record)
        {
            var result = new List<ValidationResult>();

            if (!record.PendingDate.HasValue)
            {
                result.Add(new ValidationResult(RequiredFieldMessage, new[] { nameof(record.PendingDate) }));
            }
            else if (record.PendingDate.Value > DateTime.Today.AddDays(1))
            {
                result.Add(new ValidationResult(GetErrorMessage("today", OperatorType.LessEqual), new[] { nameof(record.PendingDate) }));
            }

            if (!record.EstimatedClosedDate.HasValue)
            {
                result.Add(new ValidationResult(RequiredFieldMessage, new[] { nameof(record.EstimatedClosedDate) }));
            }
            else if (record.EstimatedClosedDate.Value < DateTime.Today)
            {
                result.Add(new ValidationResult(GetErrorMessage("today", OperatorType.GreaterEqual), new[] { nameof(record.EstimatedClosedDate) }));
            }

            result.AddValue(record.HasBuyerAgent && !record.AgentId.HasValue, new(RequiredFieldMessage, new[] { nameof(record.AgentId) }));

            return result;
        }

        private static string GetErrorMessage(string fieldName, OperatorType option)
        {
            var message = string.Empty;
            switch (option)
            {
                case OperatorType.LessThan:
                    message = $"{LessThanErrorMessage} {fieldName}";
                    break;
                case OperatorType.LessEqual:
                    message = $"{LessThanOrEqualToErrorMessage} {fieldName}";
                    break;
                case OperatorType.GreaterThan:
                    message = $"{GreaterThanErrorMessage} {fieldName}";
                    break;
                case OperatorType.GreaterEqual:
                    message = $"{GreaterThanEqualToErrorMessage} {fieldName}";
                    break;
            }

            return message;
        }
    }
}
