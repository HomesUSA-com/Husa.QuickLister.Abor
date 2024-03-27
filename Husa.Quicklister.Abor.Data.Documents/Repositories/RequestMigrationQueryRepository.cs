namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Models;
    using Husa.Quicklister.Extensions.Data.Documents.Projections;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.Azure.Cosmos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class RequestMigrationQueryRepository : ExtensionRepositories.MigrationQueryRepository<SaleListingRequest>
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly ILogger<RequestMigrationQueryRepository> logger;

        public RequestMigrationQueryRepository(
             CosmosClient cosmosClient,
             ICosmosLinqQuery cosmosLinqQuery,
             ApplicationQueriesDbContext context,
             IServiceSubscriptionClient serviceSubscriptionClient,
             IOptions<DocumentDbSettings> options,
             ILogger<RequestMigrationQueryRepository> logger)
            : base(cosmosClient, cosmosLinqQuery, serviceSubscriptionClient, options)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<IEnumerable<ListingMigrationQueryResult>> GetListingsByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            var query = this.context.ListingSale.Where(x => ids.Contains(x.Id));
            return await query.Select(ListingMigrationProjection<SaleListing>.ProjectToListingMigrationQueryResult).ToListAsync(cancellationToken);
        }

        protected override async Task<IEnumerable<ListingMigrationQueryResult>> GetListingsLockedNotSubmitted(Guid? companyId, DateTime? toModifiedOn, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Starting to get listings LockedNotSubmitted");
            var query = this.context.ListingSale.FilterNotDeleted();
            if (toModifiedOn.HasValue)
            {
                query = query.Where(x => x.SysModifiedOn < toModifiedOn.Value);
            }

            query = await this.FilterByLockedNotSubmitted(query, companyId, cancellationToken);
            return query.Select(ListingMigrationProjection<SaleListing>.ProjectToListingMigrationQueryResult);
        }
    }
}
