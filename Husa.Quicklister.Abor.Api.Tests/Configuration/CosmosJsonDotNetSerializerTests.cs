namespace Husa.Quicklister.Abor.Api.Tests.Configuration
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class CosmosJsonDotNetSerializerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SerializeFromStreamSuccess(bool enumAsInteger)
        {
            // Arrange
            var addressRecord = new AddressRecord
            {
                FormalAddress = "1 CACreated, Flour Bluff 75666",
                ReadableCity = "Flour Bluff",
                StreetNumber = "1",
                StreetName = "CACreated",
                City = Cities.Abilene,
                State = States.Texas,
                ZipCode = "75666",
                County = Counties.Andrews,
                LotNum = "2",
                Block = "2",
                Subdivision = "No SC SC is this Community",
            };
            var jsonAddressInfoStream = GetJsonStream(addressRecord, enumAsInteger);

            var sut = CosmosDbBootstrapper.BuildCosmosJsonDotNetSerializer();

            // Act
            var serializedAddress = sut.FromStream<AddressRecord>(jsonAddressInfoStream);

            // Assert
            Assert.NotNull(serializedAddress);
            Assert.Equal(addressRecord, serializedAddress);
        }

        [Fact]
        public void SerializeToStreamSuccess()
        {
            // Arrange
            var addressRecord = new AddressRecord
            {
                FormalAddress = "1 CACreated, Flour Bluff 75666",
                ReadableCity = "Flour Bluff",
                StreetNumber = "1",
                StreetName = "CACreated",
                City = Cities.Abilene,
                State = States.Texas,
                ZipCode = "75666",
                County = Counties.Anderson,
                LotNum = "2",
                Block = "2",
                Subdivision = "No SC SC is this Community",
            };

            var sut = CosmosDbBootstrapper.BuildCosmosJsonDotNetSerializer();

            // Act
            var streamAddress = sut.ToStream<AddressRecord>(addressRecord);

            // Assert
            Assert.NotNull(streamAddress);
        }

        private static Stream GetJsonStream(AddressRecord addressRecord, bool enumAsInteger = true)
        {
            var finalValue = enumAsInteger ?
                $"\"{addressRecord.State}\"" :
                $"{(int)addressRecord.State}";

            var jsonAddressInfo = $@"
            {{
                ""FormalAddress"": ""{addressRecord.FormalAddress}"",
                ""ReadableCity"": ""{addressRecord.ReadableCity}"",
                ""StreetNumber"": ""{addressRecord.StreetNumber}"",
                ""StreetName"": ""{addressRecord.StreetName}"",
                ""City"": ""{addressRecord.City}"",
                ""State"": {finalValue},
                ""ZipCode"": ""{addressRecord.ZipCode}"",
                ""County"": ""{addressRecord.County}"",
                ""LotNum"": ""{addressRecord.LotNum}"",
                ""Block"": ""{addressRecord.Block}"",
                ""Subdivision"": ""{addressRecord.Subdivision}""
            }}";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonAddressInfo);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
