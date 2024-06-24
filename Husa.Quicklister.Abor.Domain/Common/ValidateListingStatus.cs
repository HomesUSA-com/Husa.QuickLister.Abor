namespace Husa.Quicklister.Abor.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public static class ValidateListingStatus<TStatusFields>
        where TStatusFields : IProvideStatusFields
    {
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
                case MarketStatuses.Hold:
                    return HoldValidations(value);
                case MarketStatuses.Closed:
                    return SoldValidations(value);
                case MarketStatuses.Canceled:
                    return CanceledValidations(value);
                case MarketStatuses.ActiveUnderContract:
                    return ActiveUnderContractValidations(value);
            }

            return new List<ValidationResult>();
        }

        private static IEnumerable<ValidationResult> ActiveUnderContractValidations(TStatusFields record)
        {
            var result = new List<ValidationResult>();

            if (!record.PendingDate.HasValue)
            {
                result.Add(new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.PendingDate) }));
            }
            else
            {
                var pendingDate = record.PendingDate.Value;
                var tenDaysAgo = DateTime.Today.AddDays(-10);
                var tomorrow = DateTime.Today.AddDays(1);

                if (pendingDate > tomorrow)
                {
                    result.Add(new ValidationResult(OperatorType.LessEqual.GetErrorMessage("today"), new[] { nameof(record.PendingDate) }));
                }
                else if (pendingDate < tenDaysAgo)
                {
                    result.Add(new ValidationResult($"Date must be within the last 10 days.", new[] { nameof(record.PendingDate) }));
                }
            }

            if (!record.EstimatedClosedDate.HasValue)
            {
                result.Add(new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.EstimatedClosedDate) }));
            }
            else if (record.EstimatedClosedDate.Value < DateTime.Today.AddDays(1))
            {
                result.Add(new(OperatorType.GreaterEqual.GetErrorMessage("tomorrow"), new[] { nameof(record.EstimatedClosedDate) }));
            }

            if (record.ClosedDate.HasValue && record.ClosedDate.Value < DateTime.Today)
            {
                result.Add(new(OperatorType.GreaterEqual.GetErrorMessage("today"), new[] { nameof(record.ClosedDate) }));
            }

            return result;
        }

        private static IEnumerable<ValidationResult> CanceledValidations(TStatusFields record)
        {
            var result = new List<ValidationResult>();
            result.AddValue(string.IsNullOrWhiteSpace(record.CancelledReason), new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.OffMarketDate) }));
            return result;
        }

        private static IEnumerable<ValidationResult> PendingValidations(TStatusFields record)
        {
            var result = new List<ValidationResult>();

            if (!record.PendingDate.HasValue)
            {
                result.Add(new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.PendingDate) }));
            }
            else if (record.PendingDate.Value > DateTime.Today.AddDays(1))
            {
                result.Add(new(OperatorType.LessEqual.GetErrorMessage("today"), new[] { nameof(record.PendingDate) }));
            }

            if (!record.EstimatedClosedDate.HasValue)
            {
                result.Add(new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.EstimatedClosedDate) }));
            }
            else if (record.EstimatedClosedDate.Value < DateTime.Today)
            {
                result.Add(new(OperatorType.GreaterEqual.GetErrorMessage("today"), new[] { nameof(record.EstimatedClosedDate) }));
            }

            result.AddValue(record.HasBuyerAgent && !record.AgentId.HasValue, new(ErrorExtensions.RequiredFieldMessage, new[] { nameof(record.AgentId) }));

            return result;
        }

        private static IEnumerable<ValidationResult> HoldValidations(TStatusFields record)
        {
            var requiredFields = new List<string>();
            requiredFields.AddValue(!record.OffMarketDate.HasValue, nameof(record.OffMarketDate));
            requiredFields.AddValue(!record.BackOnMarketDate.HasValue, nameof(record.BackOnMarketDate));
            return ToRequiredFieldValidationResult(requiredFields);
        }

        private static IEnumerable<ValidationResult> SoldValidations(TStatusFields record)
        {
            var requiredFields = new List<string>();

            requiredFields.AddValue(!record.ClosedDate.HasValue, nameof(record.ClosedDate));
            requiredFields.AddValue(!record.PendingDate.HasValue, nameof(record.PendingDate));
            requiredFields.AddValue(!record.ClosePrice.HasValue, nameof(record.ClosePrice));
            requiredFields.AddValue(record.HasBuyerAgent && !record.AgentId.HasValue, nameof(record.AgentId));
            requiredFields.AddValue(record.HasSecondBuyerAgent && !record.AgentIdSecond.HasValue, nameof(record.AgentIdSecond));
            requiredFields.AddValue(record.SaleTerms == null || record.SaleTerms.Count < 1, nameof(record.SaleTerms));
            requiredFields.AddValue(string.IsNullOrWhiteSpace(record.SellConcess), nameof(record.SellConcess));

            var results = ToRequiredFieldValidationResult(requiredFields);
            results.AddValue(record.ClosePrice.HasValue && record.ClosePrice.Value <= 0, new(OperatorType.GreaterThan.GetErrorMessage("zero")));

            return results;
        }

        private static List<ValidationResult> ToRequiredFieldValidationResult(List<string> fields)
        {
            return fields.Count > 0 ? new List<ValidationResult> { new(ErrorExtensions.RequiredFieldMessage, fields) } : new List<ValidationResult>();
        }
    }
}
