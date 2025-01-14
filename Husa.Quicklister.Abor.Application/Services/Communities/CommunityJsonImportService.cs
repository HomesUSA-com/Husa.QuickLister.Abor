namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using CommunityExtensions = Husa.Quicklister.Extensions.Application.Services.JsonImport;

    public class CommunityJsonImportService : CommunityExtensions.CommunityJsonImportService<CommunitySale, ICommunitySaleRepository>
    {
        public CommunityJsonImportService(
            IJsonImportClient client,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<CommunityJsonImportService> logger)
            : base(client, communityRepository, userContextProvider, applicationOptions, logger)
        {
        }

        protected override Task<CommunitySale> CreateCommunity(CommunityResponse jsonCommunity, Guid companyId, string companyName)
            => Task.FromResult(new CommunitySale(
                    companyId,
                    jsonCommunity.Name,
                    companyName,
                    jsonStatus: JsonImportStatus.AwaitingApproval));

        protected override Task UpdateCommunity(CommunitySale community, CommunityResponse jsonCommunity)
        {
            community.Import(jsonCommunity);
            return Task.CompletedTask;
        }
    }
}
