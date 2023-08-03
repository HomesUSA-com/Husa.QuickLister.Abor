namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Migration.Api.Client;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Microsoft.Extensions.Logging;

    public class CommunityMigrationService : ICommunityMigrationService
    {
        private readonly ICommunitySaleRepository communityRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CommunityMigrationService> logger;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IMigrationClient migrationClient;

        public CommunityMigrationService(
            ICommunitySaleRepository communityRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CommunityMigrationService> logger,
            IMapper mapper)
        {
            this.communityRepository = communityRepository ?? throw new ArgumentNullException(nameof(communityRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.migrationClient = migrationClient ?? throw new ArgumentNullException(nameof(migrationClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task MigrateByCompanyId(Guid companyId)
        {
            var company = await this.serviceSubscriptionClient.Company.GetCompany(companyId);
            if (company == null || !company.LegacyId.HasValue)
            {
                throw new DomainException($"The company with id {companyId} is not associated to an id in v1");
            }

            this.logger.LogInformation("Starting to migrate communities from v1 related to company with id {companyId}.", companyId);
            var communitiesMigration = await this.migrationClient.Communities.GetByCompanyIdAsync(company.LegacyId.Value);

            if (!communitiesMigration.Any())
            {
                this.logger.LogInformation("Communities for company with id {companyId} where not found.", companyId);
                return;
            }

            var existingCommunityIds = (await this.communityRepository.GetByLegacyIds(communitiesMigration.Select(p => p.Id), companyId)).Select(x => x.LegacyId);

            var communities = new List<CommunitySale>();
            foreach (var communityMigration in communitiesMigration.Where(p => !existingCommunityIds.Contains(p.Id)))
            {
                var community = this.mapper.Map<CommunitySale>(communityMigration);
                var openHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communityMigration.OpenHouses);
                community.UpdateOpenHouse(openHouses);
                community.UpdateCompanyInfo(companyId, company.Name);
                communities.Add(community);
            }

            this.communityRepository.Attach(communities);
            await this.communityRepository.SaveChangesAsync();
        }
    }
}
