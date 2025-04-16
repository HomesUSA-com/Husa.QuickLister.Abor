namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
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
        protected override (bool ApplyFilter, IEnumerable<Guid> CommunityIds) GetCommunityIds(IUserContext currentUser)
            => this.GetSalesEmployeeCommunityIds<CommunitySale, CommunityEmployee, ApplicationQueriesDbContext>(currentUser, this.context);
    }
}
