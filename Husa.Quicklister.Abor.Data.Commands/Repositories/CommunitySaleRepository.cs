namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CommunitySaleRepository : Repository<CommunitySale>, ICommunitySaleRepository
    {
        public CommunitySaleRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<CommunitySaleRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<CommunitySale> GetCommunity(string name, Guid companyId)
        {
            this.logger.LogInformation($"Starting to get the Community Sale Name: {name} companyId: {companyId}");
            return await this.context.Community
                .Where(c => c.ProfileInfo.Name == name && c.CompanyId == companyId)
                .SingleOrDefaultAsync();
        }

        public async Task<CommunitySale> GetCommunityByIdAsync(Guid communityId)
        {
            this.logger.LogInformation($"Starting to get the Community Sale by Id: {communityId}");
            return await this.context.Community
                .Include(x => x.SaleProperties)
                .Where(c => c.Id == communityId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CommunitySale>> GetByLegacyIds(IEnumerable<Guid> legacyIds, Guid companyId)
        {
            this.logger.LogInformation("Starting to get the Community by legacy ids and companyId: {companyId}", companyId);
            return await this.context.Community
                .Where(p => p.LegacyId.HasValue && legacyIds.Contains(p.LegacyId.Value) && p.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Guid?> GetIdByLegacyId(Guid legacyId)
        {
            this.logger.LogInformation("Starting to get the Community by legacy id {legacyId}", legacyId);
            var community = await this.context.Community.FirstOrDefaultAsync(p => p.LegacyId.HasValue && p.LegacyId.Value == legacyId);
            return community?.Id;
        }

        public async Task<Guid?> GetIdByLegacyId(int legacyId)
        {
            this.logger.LogInformation("Starting to get the Community by legacy id {legacyId}", legacyId);
            var community = await this.context.Community.FirstOrDefaultAsync(p => p.LegacyProfileId.HasValue && p.LegacyProfileId.Value == legacyId);
            return community?.Id;
        }

        public bool IsCommunityEmployee(Guid userId, Guid communityId)
        {
            this.logger.LogInformation("Checking if user {userId} is a employee of community {communityId}", userId, communityId);
            return this.context.CommunityEmployee
                .Any(x => !x.IsDeleted && x.CommunityId == communityId && x.UserId == userId);
        }
    }
}
