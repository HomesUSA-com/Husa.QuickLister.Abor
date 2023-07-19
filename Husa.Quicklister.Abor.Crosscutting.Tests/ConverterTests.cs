namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ConverterTests
    {
#nullable enable
        [Theory]
        [InlineData("2", 2)]
        [InlineData("3+", 3)]
        [InlineData(null, null)]
        public void FireplacesConvertToIntSuccess(string? fireplace, int? expectedValue)
        {
            // Act
            var result = fireplace.FireplacesToIntConverter();

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void FireplacesConvertToIntFails()
        {
            // Arrange
            var fireplace = "4+";

            // Act & Assert
            Assert.Throws<FormatException>(() => fireplace.FireplacesToIntConverter());
        }

        [Theory]
        [InlineData("2018", 2018)]
        [InlineData("U", null)]
        [InlineData(null, null)]
        public void ConstructionStartYearToIntSuccess(string? constructionStartYear, int? expectedValue)
        {
            // Arrange
            var result = constructionStartYear.ConstructionStartYearToIntConverter();

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ConstructionStartYearToIntFails()
        {
            // Arrange
            var constructionStartYear = "YEAR";

            // Act & Assert
            Assert.Throws<FormatException>(() => constructionStartYear.FireplacesToIntConverter());
        }
    }
}
