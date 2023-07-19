namespace Husa.Quicklister.Abor.Crosscutting.Validations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class LookupAttribute : ValidationAttribute
    {
        public LookupAttribute()
           : base()
        {
            this.MarketRequired = true;
            this.MaxValue = null;
            this.MinValue = null;
            this.ValidateRange = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupAttribute"/> class.
        /// </summary>
        /// <param name="marketRequired">Market Required.</param>
        /// <param name="limitValue"> Limit for selected Options.</param>
        /// <param name="maxValue">Indicates if the Limit Value is for Higher selected options (Default: false determine is for the least amount of selected options).</param>
        public LookupAttribute(bool marketRequired, int limitValue, bool maxValue = false)
            : this()
        {
            this.MarketRequired = marketRequired;
            if (maxValue)
            {
                this.MaxValue = limitValue;
            }
            else
            {
                this.MinValue = limitValue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupAttribute"/> class.
        /// </summary>
        /// <param name="marketRequired">Market Required.</param>
        /// <param name="minValue">Min selection options for range validation.</param>
        /// <param name="maxValue">Max selection options for range validation.</param>
        public LookupAttribute(bool marketRequired, int minValue, int maxValue)
            : this()
        {
            this.MarketRequired = marketRequired;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.ValidateRange = true;
        }

        /// <summary>
        /// Gets a value indicating whether determine is field is Market Required or not.
        /// </summary>
        public bool MarketRequired { get; private set; }

        /// <summary>
        /// Gets Stablish the maximum integer value valid.
        /// </summary>
        public int? MaxValue { get; private set; }

        /// <summary>
        /// Gets Stablish the mininum  integer value valid.
        /// </summary>
        public int? MinValue { get; private set; }

        private bool ValidateRange { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] fieldValue = value != null ? value.ToString().Split(',') : null;
            var message = string.Empty;
            if (this.MarketRequired && fieldValue == null)
            {
                message = "value is required";
            }

            if (this.ValidateRange)
            {
                if (fieldValue == null)
                {
                    message = message + $"must have between {this.MinValue} and {this.MaxValue} options selected";
                }
                else if (this.MinValue > fieldValue.Count() || this.MaxValue < fieldValue.Count())
                {
                    message = $"must have  between {this.MinValue} and {this.MaxValue} options selected";
                }
            }
            else
            {
                if (this.MinValue != null)
                {
                    if (fieldValue == null)
                    {
                        message = message + $" and must have at least {this.MinValue} options selected";
                    }
                    else if (fieldValue.Count() < this.MinValue)
                    {
                        message = $"must have at least {this.MinValue} options selected";
                    }
                }

                if (this.MaxValue != null)
                {
                    if (fieldValue == null)
                    {
                        message = message + $" and must have maximum of {this.MaxValue} options selected";
                    }
                    else if (fieldValue.Count() > this.MaxValue)
                    {
                        message = $"must have maximum of {this.MaxValue} options selected";
                    }
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                return new ValidationResult(message, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
