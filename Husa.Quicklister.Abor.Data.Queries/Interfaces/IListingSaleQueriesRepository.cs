namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;

    public interface IListingSaleQueriesRepository : IQueryListingRepository
    {
        Task<DataSet<ListingSaleQueryResult>> GetAsync(ListingQueryFilter queryFilter);

        Task<ListingSaleQueryDetailResult> GetListing(Guid listingId);

        Task<ReversePorspectListingQueryResult> GetReverseProspectListing(Guid listingId, Guid userId, Guid companyId);

        Task<ListingSaleQueryDetailResult> GetListingByAddress(string streetName, string streetNumber, string zipCode);

        Task<DataSet<SaleListingOpenHouseQueryResult>> GetListingsWithOpenHouse(BaseQueryFilter queryFilter);
    }
}
