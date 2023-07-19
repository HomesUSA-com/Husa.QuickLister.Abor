namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ScrapedListingQueriesRepository : IScrapedListingQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly ILogger<ScrapedListingQueriesRepository> logger;

        public ScrapedListingQueriesRepository(
            ApplicationQueriesDbContext context,
            ILogger<ScrapedListingQueriesRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ScrapedListingQueryResult>> GetAsync(ScrapedListingQueryFilter queryFilter)
        {
            var filterOptions = !queryFilter.BuilderName.Equals(string.Empty) ? $"of builder name {queryFilter.BuilderName}" : "of all companies";
            this.logger.LogInformation("Starting to get the ABOR comparison report {filterOptions}", filterOptions);

            return await (from listing in this.context.ScrapedListing
                          where listing.ListingDetails.BuilderName == queryFilter.BuilderName
                          select new ScrapedListingQueryResult
                          {
                              Id = listing.Id,
                              OfficeName = listing.ListingDetails.OfficeName,
                              BuilderName = listing.ListingDetails.BuilderName,
                              UncleanBuilder = listing.ListingDetails.UncleanBuilder,
                              DOM = listing.ListingDetails.DOM,
                              MlsNum = listing.ListingDetails.MlsNum,
                              ListStatus = listing.ListingDetails.ListStatus,
                              Community = listing.ListingDetails.Community,
                              Address = listing.ListingDetails.Address,
                              City = listing.ListingDetails.City,
                              ListDate = listing.ListingDetails.ListDate,
                              Refreshed = listing.ListingDetails.Refreshed,
                              Price = listing.ListingDetails.Price,
                              ListPrice = listing.ListingDetails.ListPrice,
                              Variance = (int?)((listing.ListingDetails.Price > 0 && listing.ListingDetails.ListPrice > 0) ?
                                            listing.ListingDetails.Price - listing.ListingDetails.ListPrice : null),
                          })
                          .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take)
                          .ApplySortByFields(queryFilter.SortBy)
                          .ToListAsync();
        }
    }
}
