namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public interface ILotListingQueriesRepository
    {
        Task<DataSet<LotListingQueryResult>> GetAsync(ListingQueryFilter queryFilter);

        Task<LotListingQueryDetailResult> GetListing(Guid listingId);
    }
}
