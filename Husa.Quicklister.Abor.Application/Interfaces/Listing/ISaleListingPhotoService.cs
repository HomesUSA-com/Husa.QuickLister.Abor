namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using ExtensionInterface = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

    public interface ISaleListingPhotoService : ExtensionInterface.IListingPhotoService<SaleListing>
    {
    }
}
