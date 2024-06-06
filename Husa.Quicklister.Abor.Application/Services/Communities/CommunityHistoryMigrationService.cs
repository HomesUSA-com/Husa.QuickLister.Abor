namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Migration.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;

    public class CommunityHistoryMigrationService : ExtensionsServices.CommunityHistoryMigrationService<CommunitySale, ICommunitySaleRepository>
    {
        private readonly IMapper mapper;
        private readonly ICommunityHistoryService communityHistoryService;
        private readonly ICommunityHistoryRepository communityHistoryRepository;
        private readonly ICommunityMigrationService communityMigrationService;

        public CommunityHistoryMigrationService(
            ICommunityHistoryService communityHistoryService,
            ICommunityHistoryRepository communityHistoryRepository,
            ICommunitySaleRepository communityRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            IMigrationClient migrationClient,
            ICommunityMigrationService communityMigrationService,
            ILogger<CommunityHistoryMigrationService> logger,
            IMapper mapper)
            : base(communityRepository, serviceSubscriptionClient, userContextProvider, migrationClient, logger)
        {
            this.communityHistoryService = communityHistoryService ?? throw new ArgumentNullException(nameof(communityHistoryService));
            this.communityHistoryRepository = communityHistoryRepository ?? throw new ArgumentNullException(nameof(communityHistoryRepository));
            this.communityMigrationService = communityMigrationService ?? throw new ArgumentNullException(nameof(communityMigrationService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override MigrationMarketType MigrationMarket => MigrationMarketType.Austin;

        protected override async Task GenerateRecordAsync(Guid currentUserId, CommunitySale community, CommunityHistoryResponse legacyRequest, bool updateRequest = false)
        {
            var existingRecord = await this.communityHistoryRepository.GetByLegacyIdAsync(legacyRequest.LegacyCommunityHistoryId);
            if (!updateRequest && existingRecord != null)
            {
                return;
            }

            var record = this.mapper.Map<CommunityHistory>(legacyRequest);
            record.Id = Guid.NewGuid();
            record.EntityId = community.Id;
            record.CompanyId = community.CompanyId;

            var openHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(community.OpenHouses);
            record.AddOpenHouses(openHouses);

            var usersIds = await this.GetMigrationUserIds(legacyRequest);
            record.SysCreatedBy = usersIds.SysCreatedBy ?? currentUserId;
            record.SysModifiedBy = usersIds.SysModifiedBy ?? currentUserId;

            record.SysCreatedOn = legacyRequest.SysCreatedOn;
            record.SysModifiedOn = legacyRequest.SysModifiedOn;
            record.LegacyId = legacyRequest.LegacyCommunityHistoryId;

            if (existingRecord != null)
            {
                await this.communityHistoryRepository.UpdateDocumentAsync(existingRecord.Id, record, currentUserId);
                return;
            }

            await this.communityHistoryService.AddRecordAsync(record);
            await this.UpdateCommunityInformation(community, legacyRequest);
        }

        protected async Task UpdateCommunityInformation(CommunitySale community, CommunityHistoryResponse legacyRecord)
        {
            if (legacyRecord.SysModifiedOn > community.SysModifiedOn)
            {
                this.communityMigrationService.UpdateCommunity(community, legacyRecord);
                await this.CommunityRepository.SaveChangesAsync(community);
            }
        }
    }
}
