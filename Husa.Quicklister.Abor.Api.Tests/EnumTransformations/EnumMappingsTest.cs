namespace Husa.Quicklister.Abor.Api.Tests.EnumTransformations
{
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Api.Mappings.EnumTransformations;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;
    using Trestle = Husa.Downloader.CTX.Domain.Enums;

    [ExcludeFromCodeCoverage]
    public class EnumMappingsTest
    {
        [Theory]
        [InlineData(Trestle.Levels.One, Stories.One)]
        [InlineData(Trestle.Levels.OneandOneHalf, Stories.OnePointFive)]
        [InlineData(Trestle.Levels.Two, Stories.Two)]
        [InlineData(Trestle.Levels.ThreeOrMore, Stories.ThreePlus)]
        [InlineData(Trestle.Levels.MultiLevelUnit, Stories.MultiLevel)]
        [InlineData((Trestle.Levels)999, null)]
        public void ToStories_ValidLevels_ReturnsExpectedStories(Trestle.Levels input, Stories? expected)
        {
            // Act
            var result = EnumMappings.ToStories(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToStories_NullInput_ReturnsNull()
        {
            // Arrange
            Trestle.Levels? input = null;

            // Act
            var result = EnumMappings.ToStories(input);

            // Assert
            Assert.Null(result);
        }
    }
}
