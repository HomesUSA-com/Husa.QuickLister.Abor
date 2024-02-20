namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Communities;

    public class CommunityHistoryService : ExtensionsServices.CommunityHistoryService<
        CommunitySale,
        CommunityHistory,
        ICommunitySaleRepository,
        ICommunityHistoryRepository>
    {
        public CommunityHistoryService(
            ICommunitySaleRepository saleCommunityRepository,
            ICommunityHistoryRepository communityHistoryRepository,
            IUserContextProvider userContextProvider,
            ILogger<CommunityHistoryService> logger)
            : base(
                  saleCommunityRepository,
                  communityHistoryRepository,
                  userContextProvider,
                  logger)
        {
        }
    }
}
