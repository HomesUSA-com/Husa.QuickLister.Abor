namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;

    public interface ICommunityQueriesRepository
    {
        Task<DataSet<CommunityQueryResult>> GetAsync(ProfileQueryFilter queryFilter);

        Task<CommunityDetailQueryResult> GetCommunityById(Guid id);

        Task<CommunityDetailQueryResult> GetCommunityByName(Guid companyId, string communityName);

        Task<DataSet<CommunityEmployeeQueryResult>> GetCommunityEmployees(Guid communityId, string sortBy);

        Task<CommunityDetailQueryResult> GetByIdWithListingImportProjection(Guid id, Guid listingId);
    }
}
