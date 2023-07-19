namespace Husa.Quicklister.Abor.Application.Tests
{
    using Xunit;

    [CollectionDefinition("Husa.Quicklister.Abor.Application.Test")]
    public class ApplicationServicesCollection : ICollectionFixture<ApplicationServicesFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
