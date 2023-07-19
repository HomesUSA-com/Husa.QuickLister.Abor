namespace Husa.Quicklister.Abor.Api.Contracts.Tests
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Api.Contracts.ValidationAttributes;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Crosscutting.Test")]
    public class RequiredIfNullAttributeTests
    {
        [Fact]
        public void RequiredIfNullReturnTrue()
        {
            // Arrange
            var instance = new { AttributeOne = "AttributeOne", AttributeTwo = (string)null };
            var requiredIfNullAttribute = new RequiredIfNullAttribute("AttributeOne");
            var context = new ValidationContext(instance);

            // Act
            var result = requiredIfNullAttribute.GetValidationResult(instance.AttributeTwo, context);

            // Assert
            Assert.True(result == ValidationResult.Success);
        }

        [Fact]
        public void RequiredIfNullReturnFalse()
        {
            // Arrange
            var instance = new { AttributeOne = (string)null, AttributeTwo = (string)null };
            var requiredIfNullAttribute = new RequiredIfNullAttribute("AttributeOne");
            var context = new ValidationContext(instance);

            // Act
            var result = requiredIfNullAttribute.GetValidationResult(instance.AttributeTwo, context);

            // Assert
            Assert.False(result == ValidationResult.Success);
        }
    }
}
