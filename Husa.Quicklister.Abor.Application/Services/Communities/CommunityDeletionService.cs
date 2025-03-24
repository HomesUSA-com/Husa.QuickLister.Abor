namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Communities;

    public class CommunityDeletionService : ExtensionsServices.CommunityDeletionService<CommunitySale>
    {
        public CommunityDeletionService(
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            IJsonImportClient importClient,
            ILogger<CommunityDeletionService> logger)
            : base(communityRepository, userContextProvider, importClient, logger)
        {
        }
    }
}
