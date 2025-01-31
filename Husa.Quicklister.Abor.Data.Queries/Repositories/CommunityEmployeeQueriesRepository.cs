namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Models;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class CommunityEmployeeQueriesRepository : QueryCommunityEmployeeRepository<ApplicationQueriesDbContext, CommunityEmployee>
    {
        public CommunityEmployeeQueriesRepository(IUserRepository userQueriesRepository, ILogger<CommunityEmployeeQueriesRepository> logger, ApplicationQueriesDbContext context)
            : base(userQueriesRepository, logger, context)
        {
        }

        protected override Expression<Func<CommunityEmployee, CommunityEmployeeQueryResult>> ProjectToCommunityEmployeeQueryResult
            => CommunityProjection.ProjectionToCommunityEmployeeQueryResult;
    }
}
