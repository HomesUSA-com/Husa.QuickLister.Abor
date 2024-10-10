namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using RepositoryExtensions = Husa.Quicklister.Extensions.Data.Queries.Repositories.QueryJsonRepository;
    public class QueryJsonRepository : RepositoryExtensions
    {
        private readonly ApplicationQueriesDbContext context;
        public QueryJsonRepository(
            ApplicationQueriesDbContext context, IJsonImportClient xmlClient, IUserContextProvider userContext, IMapper mapper)
             : base(xmlClient, userContext, mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override MarketCode MarketCode => MarketCode.Austin;
        protected override IEnumerable<Guid> GetUserCommunityIds(IUserContext currentUser)
        => this.context.Community
                    .FilterNotDeleted()
                    .FilterByJsonImportStatus()
                    .FilterByCompany(currentUser)
                    .Join(this.context.CommunityEmployee, comm => comm.Id, emp => emp.CommunityId, (communities, employee) => new { communities, employee })
                    .Where(employeeCommunties => !employeeCommunties.employee.IsDeleted && employeeCommunties.employee.UserId == currentUser.Id)
                    .Select(employeeCommunties => employeeCommunties.communities.Id)
                    .ToList();
    }
}
