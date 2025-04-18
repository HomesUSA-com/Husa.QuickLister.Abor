namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Xml.Api.Client.Interface;
    using RepositoryExtensions = Husa.Quicklister.Extensions.Data.Queries.Repositories.QueryXmlRepository;

    public class QueryXmlRepository : RepositoryExtensions
    {
        private readonly ApplicationQueriesDbContext context;

        public QueryXmlRepository(
            ApplicationQueriesDbContext context, IXmlClient xmlClient, IUserContextProvider userContext, IMapper mapper)
             : base(xmlClient, userContext, mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override MarketCode MarketCode => MarketCode.Austin;

        protected override (bool ApplyFilter, IEnumerable<Guid> CommunityIds) GetCommunityIds(IUserContext currentUser)
            => this.GetSalesEmployeeCommunityIds<CommunitySale, CommunityEmployee, ApplicationQueriesDbContext>(currentUser, this.context);
    }
}
