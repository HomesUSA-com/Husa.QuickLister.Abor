namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq.Expressions;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Enums;
    using Husa.MediaService.Client;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Crosscutting.Clients;
    using ExtensionsRepositories = Husa.Quicklister.Extensions.Data.Queries.Repositories;

    public class ResidentialIdxQueriesRepository : ExtensionsRepositories.ResidentialIdxQueriesRepository<SaleListing, ApplicationQueriesDbContext, ResidentialIdxQueryResult>, IResidentialIdxQueriesRepository
    {
        public ResidentialIdxQueriesRepository(
            ApplicationQueriesDbContext context,
            IXmlClientWithToken xmlClient,
            IMediaServiceClient mediaClient,
            IServiceSubscriptionClient serviceSubscriptionClient)
            : base(context, xmlClient, mediaClient, serviceSubscriptionClient)
        {
        }

        protected override MarketCode MarketCode => MarketCode.Austin;
        protected override Expression<Func<SaleListing, ResidentialIdxQueryResult>> ToResidentialIdxQueryResultProjection => ResidentialIdxProjection.ToResidentialIdxQueryResult;
    }
}
