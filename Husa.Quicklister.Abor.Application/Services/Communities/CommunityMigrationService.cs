namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;

    public class CommunityMigrationService : ExtensionsServices.CommunityMigrationService<CommunitySale, ICommunitySaleRepository>
    {
        private readonly IMapper mapper;

        public CommunityMigrationService(
            ICommunitySaleRepository communityRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CommunityMigrationService> logger,
            IMapper mapper)
            : base(communityRepository, migrationClient, serviceSubscriptionClient, logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override CommunitySale CreateCommunity(CompanyDetail company, CommunityResponse communityMigration)
        {
            var community = this.mapper.Map<CommunitySale>(communityMigration);
            var openHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communityMigration.OpenHouses);
            community.UpdateOpenHouse(openHouses);
            community.UpdateCompanyInfo(company.Id, company.Name);
            community.Showing.OwnerName = company.Name;
            return community;
        }
    }
}
