namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;

    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ConvertToCentral_GetUTCDate_Success()
        {
            // Arrange
            var date = DateTime.Now;

            // Act
            var result = date.ConvertToCentral();

            // Assert
            Assert.IsType<DateTime>(result);
            Assert.NotEqual(date, result);
        }

        [Theory]
        [InlineData(100, MlsArea.Hundred)]
        [InlineData(101, MlsArea.HundredOne)]
        [InlineData(102, MlsArea.HundredTwo)]
        [InlineData(103, MlsArea.HundredThree)]
        [InlineData(104, MlsArea.HundredFour)]
        [InlineData(105, MlsArea.HundredFive)]
        [InlineData(200, MlsArea.TwoHundred)]
        [InlineData(300, MlsArea.ThreeHundred)]
        [InlineData(400, MlsArea.FourHundred)]
        [InlineData(500, MlsArea.FiveHundred)]
        public void CastToMlsAreaSuccess(int numericMlsArea, MlsArea expectedMlsArea)
        {
            // Act
            var result = (MlsArea?)numericMlsArea;

            // Assert
            Assert.IsType<MlsArea>(result);
            Assert.Equal(expectedMlsArea, result);
        }
    }
}
