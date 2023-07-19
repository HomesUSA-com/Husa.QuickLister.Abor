namespace Husa.Quicklister.Abor.Api.Tests.Configuration
{
    using Xunit;

    [CollectionDefinition(nameof(ApplicationServicesFixture))]
    public class ApplicationServicesCollection : ICollectionFixture<ApplicationServicesFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
