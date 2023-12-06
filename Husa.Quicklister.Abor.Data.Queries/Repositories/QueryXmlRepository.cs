namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Xml;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public class QueryXmlRepository : IQueryXmlRepository
    {
        private readonly IXmlClient xmlClient;
        private readonly IMapper mapper;
        private readonly IUserContextProvider userContext;
        private readonly ApplicationQueriesDbContext context;

        public QueryXmlRepository(
            ApplicationQueriesDbContext context, IXmlClient xmlClient, ICommunityQueriesRepository communityQueriesRepository, IUserContextProvider userContext, IMapper mapper)
        {
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<DataSet<XmlResponse.XmlListingResponse>> GetAsync(XmlListingQueryFilter filter)
        {
            var xmlFilter = this.mapper.Map<ListingRequestFilter>(filter);
            xmlFilter.MarketCode = MarketCode.Austin;
            var currentUser = this.userContext.GetCurrentUser();

            if (currentUser.UserRole == UserRole.User && currentUser.EmployeeRole == RoleEmployee.SalesEmployee)
            {
                var communityIds = this.context.Community
                    .FilterNotDeleted()
                    .FilterByImportStatus(XmlStatus.Approved)
                    .FilterByCompany(currentUser)
                    .Join(this.context.CommunityEmployee, comm => comm.Id, emp => emp.CommunityId, (communities, employee) => new { communities, employee })
                    .Where(employeeCommunties => !employeeCommunties.employee.IsDeleted && employeeCommunties.employee.UserId == currentUser.Id)
                    .Select(employeeCommunties => employeeCommunties.communities.Id)
                    .ToList();
                if (!communityIds.Any())
                {
                    return Task.FromResult(new DataSet<XmlResponse.XmlListingResponse>(Array.Empty<XmlResponse.XmlListingResponse>(), 0));
                }

                xmlFilter.CommunityIds = communityIds;
            }

            return this.xmlClient.Listing.GetAsync(xmlFilter);
        }
    }
}
