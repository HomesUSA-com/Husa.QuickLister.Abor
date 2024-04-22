namespace Husa.Quicklister.Abor.Application.Interfaces.Lot
{
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using ExtensionInterface = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

    public interface ILotListingPhotoService : ExtensionInterface.IListingPhotoService<LotListing>
    {
    }
}
