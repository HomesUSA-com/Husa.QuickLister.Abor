namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using Xunit;

    [CollectionDefinition("Husa.Quicklister.Abor.Api.Client.Tests")]
    public class ApplicationServicesCollection : ICollectionFixture<CustomWebApplicationFactory<TestStartup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
