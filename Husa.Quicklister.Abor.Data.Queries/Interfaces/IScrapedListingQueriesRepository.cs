namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public interface IScrapedListingQueriesRepository
    {
        Task<IEnumerable<ScrapedListingQueryResult>> GetAsync(ScrapedListingQueryFilter queryFilter);
    }
}
