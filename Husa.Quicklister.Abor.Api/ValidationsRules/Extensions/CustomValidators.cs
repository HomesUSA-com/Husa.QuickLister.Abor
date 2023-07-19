namespace Husa.Quicklister.Abor.Api.ValidationsRules.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Husa.Quicklister.Abor.Crosscutting;

    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, int?> ValidateYear<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, yearValue, context) =>
            {
                return DateTime.TryParse($"1/1/{yearValue}", provider: ApplicationOptions.ApplicationCultureInfo, out DateTime checkDate);
            }).WithMessage("{PropertyName} is not a valid year.");
        }

        public static IRuleBuilderOptionsConditions<T, IEnumerable<TElement>> ValidateListCount<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, int mincount)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (list.Count() < mincount)
                {
                    context.AddFailure("The {PropertyName} must contain at least " + mincount.ToString() + "items");
                }
            });
        }
    }
}
