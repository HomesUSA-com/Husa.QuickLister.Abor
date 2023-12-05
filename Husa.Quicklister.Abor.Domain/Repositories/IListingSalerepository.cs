namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Repositories;

    public interface IListingSaleRepository : ISaleListingRepository<SaleListing>
    {
        Task<SaleListing> GetListingByLocationAsync(string mlsNumber, string streetNumber, string streetName, string zip);

        Task<SaleListing> GetListing(string streetNumber, string streetName, Cities city, string zipcode, string unitNumber = null);

        Task<SaleListing> GetListingByMlsNumber(Guid listingId, string mlsNumber);

        Task<SaleListing> GetListingByXmlListingId(Guid xmlListingId);

        Task<IEnumerable<SaleListing>> GetAutmaticMatchingListingsAsync(string streetName, string streetNumber, string zipCode, Guid companyId, bool partialMatch = false);
    }
}
