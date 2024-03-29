namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using ExtensionsInterface = Husa.Quicklister.Extensions.Application.Interfaces.Migration;

    public interface ISaleListingMigrationService : ExtensionsInterface.ISaleListingMigrationService<SaleListing>
    {
    }
}
