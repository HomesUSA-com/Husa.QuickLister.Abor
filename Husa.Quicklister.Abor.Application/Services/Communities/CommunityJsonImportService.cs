namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Microsoft.Extensions.Logging;
    using CommunityExtensions = Husa.Quicklister.Extensions.Application.Services.Communities;

    public class CommunityJsonImportService : CommunityExtensions.CommunityJsonImportService<CommunitySale, ICommunitySaleRepository>
    {
        public CommunityJsonImportService(
            IJsonImportClient client,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            ILogger<CommunityJsonImportService> logger)
            : base(client, communityRepository, userContextProvider, logger)
        {
        }

        protected override async Task<CommunitySale> CreateOrUpdateCommunity(CommunityResponse jsonCommunity, Guid companyId, string companyName)
        {
            CommunitySale community;

            if (jsonCommunity.QuicklisterId.HasValue && jsonCommunity.QuicklisterId != Guid.Empty)
            {
                community = await this.CommunityRepository.GetById(jsonCommunity.QuicklisterId.Value, filterByCompany: true) ??
                    throw new NotFoundException<CommunitySale>(jsonCommunity.QuicklisterId);
            }
            else
            {
                community = new CommunitySale(
                    companyId,
                    jsonCommunity.Name,
                    companyName,
                    jsonStatus: JsonImportStatus.AwaitingApproval);
                this.CommunityRepository.Attach(community);
            }

            return community;
        }
    }
}
