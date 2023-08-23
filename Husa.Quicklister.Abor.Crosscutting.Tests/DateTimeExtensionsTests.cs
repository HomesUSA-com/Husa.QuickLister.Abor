namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
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
    }
}
