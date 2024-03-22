namespace Husa.Quicklister.Abor.Api.Tests.Mappings
{
    using Husa.Quicklister.Abor.Api.Mappings.Migration;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;

    public class MigrationExtensionsTests
    {
        [Fact]
        public void ToFireplaceDescription_Success()
        {
            var value = "FBTHR,FBDRM,FLVRM,WDBRN";

            var result = value.ToFireplaceDescription();

            // Assert
            Assert.Contains(FireplaceDescription.Bathroom, result);
            Assert.Contains(FireplaceDescription.Bedroom, result);
            Assert.Contains(FireplaceDescription.LivingRoom, result);
            Assert.Contains(FireplaceDescription.WoodBurning, result);
        }

        [Fact]
        public void ToMarketStatuses_Success()
        {
            var value = "Withdrawn";

            var result = value.ToMarketStatuses();

            // Assert
            Assert.Equal(MarketStatuses.Canceled, result);
        }
    }
}
