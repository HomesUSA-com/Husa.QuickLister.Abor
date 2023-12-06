namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Repositories;

    public interface IListingSaleRepository : ISaleListingRepository<SaleListing>
    {
        Task<SaleListing> GetListing(string streetNumber, string streetName, Cities city, string zipcode, string unitNumber = null);

        Task<SaleListing> GetListingByMlsNumber(Guid listingId, string mlsNumber);
        bool HasXmlChanges(SaleListing entity);
    }
}
