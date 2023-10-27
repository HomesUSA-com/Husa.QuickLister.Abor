namespace Husa.Quicklister.Abor.Api.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class MappingTests
    {
        private readonly ApplicationServicesFixture fixture;

        public MappingTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void ListingMappingTest()
        {
            var residentialResponse = TestModelProvider.GetResidentialResponse();
            var listingSaleDto = this.fixture.Mapper.Map<FullListingSaleDto>(residentialResponse);

            Assert.NotNull(listingSaleDto);
            Assert.NotNull(listingSaleDto.SaleProperty.FinancialInfo);
            Assert.NotNull(listingSaleDto.SaleProperty.ShowingInfo);
        }
    }
}
