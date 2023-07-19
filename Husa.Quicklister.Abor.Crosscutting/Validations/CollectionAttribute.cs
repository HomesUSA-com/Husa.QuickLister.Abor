namespace Husa.Quicklister.Abor.Crosscutting.Validations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CollectionAttribute : ValidationAttribute
    {
        private readonly bool validateRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionAttribute"/> class.
        /// Set Property Market Required.
        /// </summary>
        public CollectionAttribute()
            : base()
        {
            this.MarketRequired = true;
            this.MaxLength = null;
            this.MinLength = null;
            this.validateRange = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionAttribute"/> class.
        /// Gets Configure property validation with a limit value and use <paramref name="maxLimit"/> to set it as Max or Min.
        /// </summary>
        /// <param name="marketRequired">is Market Required.</param>
        /// <param name="limitValue">Value limit to use for validations.</param>
        /// <param name="maxLimit">True to indicate <paramref name="limitValue"/> must be used for max options required otherwise is used as min options required for validation. </param>
        public CollectionAttribute(bool marketRequired, int limitValue, bool maxLimit = false)
            : this()
        {
            this.MarketRequired = marketRequired;
            if (maxLimit)
            {
                this.MaxLength = limitValue;
            }
            else
            {
                this.MinLength = limitValue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionAttribute"/> class.
        /// Configure property range validation.
        /// </summary>
        /// <param name="marketRequired">Is Market Required.</param>
        /// <param name="maxLength">Min options Required.</param>
        /// <param name="minLength">Max options Require.</param>
        public CollectionAttribute(bool marketRequired, int maxLength = 0, int minLength = 0)
            : this()
        {
            this.MarketRequired = marketRequired;
            this.MaxLength = maxLength != 0 ? maxLength : null;
            this.MinLength = minLength != 0 ? minLength : null;
            this.validateRange = true;
        }

        /// <summary>
        /// Gets a value indicating whether determine is field is Market Required or not.
        /// </summary>
        public bool MarketRequired { get; private set; }

        /// <summary>
        /// Gets stablish the maximum valid longitude.
        /// </summary>
        public int? MaxLength { get; private set; }

        /// <summary>
        /// Gets Stablish the mininum valid longitude.
        /// </summary>
        public int? MinLength { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ICollection<object> coll = (ICollection<object>)value;
            var message = string.Empty;
            if (this.MarketRequired && (coll == null || coll.Count < 1))
            {
                message = "is required";
            }

            if (this.validateRange)
            {
                if (coll == null || coll.Count < 1)
                {
                    message = $" and selected options must be between {this.MinLength} and {this.MaxLength} values";
                }
                else if (this.MinLength > coll.Count || this.MaxLength < coll.Count)
                {
                    message = $"Selected options must be between {this.MinLength} and {this.MaxLength} values";
                }
            }
            else
            {
                if (this.MinLength != null)
                {
                    if (coll == null || coll.Count < 1)
                    {
                        message = message + $" and must have more options than {this.MinLength}";
                    }
                    else if (coll.Count < this.MinLength)
                    {
                        message = $"must have more options than {this.MinLength}";
                    }
                }

                if (this.MaxLength != null)
                {
                    if (coll == null || coll.Count < 1)
                    {
                        message = message + $" and must have less options than {this.MaxLength}";
                    }
                    else if (coll.Count > this.MaxLength)
                    {
                        message = $"value must have less options than {this.MaxLength}";
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
