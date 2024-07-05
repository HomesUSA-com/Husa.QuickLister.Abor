namespace Husa.Quicklister.Abor.Api.ValidationsRules
{
    using System;
    using FluentValidation;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSaleRequestWithStatusChangeValidator : AbstractValidator<ListingSaleRequestForUpdate>, IValidateListingStatusChanges<ListingSaleRequestForUpdate>
    {
        private const string RequiredFieldMessage = "This field is required";
        private const string RequiredValueFieldMessage = "{PropertyName} value is required";
        private const string GreaterThanErrorMessage = "This field must be greater than";
        private const string GreaterThanEqualToErrorMessage = "This field must be greater or equal to";
        private const string LessThanErrorMessage = "This field must be less than";
        private const string LessThanOrEqualToErrorMessage = "This field must be less or equal to";

        private const string LessThan = "lessThan";
        private const string LessThanOrEqualTo = "lessThanOrEqualTo";
        private const string GreaterThan = "greatedThan";
        private const string GreaterThanOrEqualTo = "greaterThanOrEqualTo";

        public ListingSaleRequestWithStatusChangeValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = ApplicationOptions.ApplicationCultureInfo;

            // Rules for MlsStatus
            this.RuleFor(f => f.MlsStatus)
                .IsInEnum().WithMessage("{PropertyName} value is not valid");
            this.ValidateRequestGeneration();
            this.ValidationsRulesForChangeStatusToPending();
            this.ValidationsRulesForChangeStatusToCancelled();
            this.ValidationsRulesForChangeStatusToHold();
            this.ValidationsRulesForChangeStatusToClosed();
            this.ValidationsRulesForChangeStatusToActiveUnderContract();
        }

        public void ValidationsRulesForChangeStatusToCancelled()
        {
            this.When(l => l.MlsStatus == MarketStatuses.Canceled, () =>
            {
                this.RuleFor(f => f.StatusFieldsInfo.CancelledReason)
                    .NotEmpty().WithMessage(RequiredFieldMessage);
            });
        }

        public void ValidationsRulesForChangeStatusToActiveUnderContract()
        {
            this.When(l => l.MlsStatus == MarketStatuses.ActiveUnderContract, () =>
            {
                this.RuleFor(f => f.StatusFieldsInfo.PendingDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage)
                    .LessThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage(GetErrorMessage("today", LessThanOrEqualTo));

                this.RuleFor(f => f.StatusFieldsInfo.EstimatedClosedDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage)
                    .GreaterThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage(GetErrorMessage("tomorrow", GreaterThanOrEqualTo));

                this.RuleFor(f => f.StatusFieldsInfo.ClosedDate)
                    .GreaterThanOrEqualTo(DateTime.Today).WithMessage(GetErrorMessage("today", GreaterThanOrEqualTo));
            });
        }

        public void ValidationsRulesForChangeStatusToPending()
        {
            this.When(l => l.MlsStatus == MarketStatuses.Pending, () =>
            {
                this.RuleFor(f => f.StatusFieldsInfo.EstimatedClosedDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage)
                    .GreaterThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage(GetErrorMessage("tomorrow", GreaterThanOrEqualTo));

                this.RuleFor(f => f.StatusFieldsInfo.PendingDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage)
                    .LessThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage(GetErrorMessage("today", LessThanOrEqualTo));
            });
        }

        public void ValidationsRulesForChangeStatusToClosed()
        {
            this.When(l => l.MlsStatus == MarketStatuses.Closed, () =>
            {
                this.RuleFor(f => f.StatusFieldsInfo.PendingDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage)
                    .LessThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage(GetErrorMessage("today", LessThanOrEqualTo));
                this.RuleFor(f => f.StatusFieldsInfo.ClosedDate)
                    .NotEmpty().WithMessage(RequiredFieldMessage);
                this.RuleFor(f => f.StatusFieldsInfo.ClosePrice)
                    .NotNull().WithMessage(RequiredFieldMessage)
                    .GreaterThan(0).WithMessage(GetErrorMessage("zero", GreaterThan));
                this.RuleFor(f => f.StatusFieldsInfo.SellConcess)
                    .NotEmpty().WithMessage(RequiredFieldMessage);
                this.RuleFor(f => f.StatusFieldsInfo.SaleTerms)
                    .NotEmpty().WithMessage(RequiredFieldMessage);
            });
        }

        public void ValidationsRulesForChangeStatusToHold()
        {
            this.When(l => l.MlsStatus == MarketStatuses.Hold, () =>
            {
                this.RuleFor(f => f.StatusFieldsInfo.OffMarketDate).NotEmpty().WithMessage(RequiredFieldMessage);
                this.RuleFor(f => f.StatusFieldsInfo.BackOnMarketDate).NotEmpty().WithMessage(RequiredFieldMessage);
            });
        }

        public void ValidateRequestGeneration()
        {
            this.RuleFor(x => x.ListPrice).NotNull().WithMessage(RequiredValueFieldMessage);
            this.RuleFor(x => x.SaleProperty).SetValidator(new SalePropertyValidator());
        }

        private static string GetErrorMessage(string fieldName, string option)
        {
            var message = string.Empty;
            switch (option)
            {
                case LessThan:
                    message = $"{LessThanErrorMessage} {fieldName}";
                    break;
                case LessThanOrEqualTo:
                    message = $"{LessThanOrEqualToErrorMessage} {fieldName}";
                    break;
                case GreaterThan:
                    message = $"{GreaterThanErrorMessage} {fieldName}";
                    break;
                case GreaterThanOrEqualTo:
                    message = $"{GreaterThanEqualToErrorMessage} {fieldName}";
                    break;
            }

            return message;
        }
    }
}
