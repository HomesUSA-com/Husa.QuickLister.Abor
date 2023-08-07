namespace Husa.Quicklister.Abor.Api.Tests.Converters
{
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Xunit;

    public class HoaConverterTests
    {
        [Theory]
        [InlineData("A", BillingFrequency.Annually)]
        [InlineData("Q", BillingFrequency.Quarterly)]
        [InlineData("MN", BillingFrequency.Monthly)]
        [InlineData("SEMIA", BillingFrequency.SemiAnnually)]
        public void Convert_WhenHoaMessageIsNotNull_ShouldReturnHoaDto(string enumMemberValue, BillingFrequency billingFrequency)
        {
            // Arrange
            var hoaMessage = new HoaMessage
            {
                Name = "Hoa Name",
                TransferFee = 100,
                Fee = 200,
                BillingFrequency = enumMemberValue,
                Website = "http://www.hoa.com",
                ContactPhone = "123456789",
                EntityOwnerType = "Hoa Type",
            };
            var sut = new HoaConverter();

            // Act
            var result = sut.Convert(hoaMessage, destination: null, context: null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hoaMessage.Name, result.Name);
            Assert.Equal(hoaMessage.TransferFee, result.TransferFee);
            Assert.Equal(hoaMessage.Fee, result.Fee);
            Assert.Equal(billingFrequency, result.BillingFrequency);
            Assert.Equal(hoaMessage.Website, result.Website);
            Assert.Equal(hoaMessage.ContactPhone, result.ContactPhone);
            Assert.Equal(hoaMessage.EntityOwnerType, result.HoaType);
        }
    }
}
