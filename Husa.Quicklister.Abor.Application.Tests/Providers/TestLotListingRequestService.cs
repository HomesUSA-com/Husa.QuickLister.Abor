namespace Husa.Quicklister.Abor.Application.Tests.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Application.Interfaces.Uploader;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class TestLotListingRequestService : LotListingRequestService
    {
        public TestLotListingRequestService(ILotListingRequestRepository saleRequestRepository, ILotListingRepository listingRepository, IUploaderService uploaderService, ILotListingRequestMediaService mediaService, IUserContextProvider userContextProvider, ICommunitySaleRepository saleCommunityRepository, IMapper mapper, ILogger<LotListingRequestService> logger, IOptions<ApplicationOptions> options, IEmailService emailService, IUserRepository userRepository, IProvideShowingTimeContacts showingTimeContactsProvider)
            : base(saleRequestRepository, listingRepository, uploaderService, mediaService, userContextProvider, saleCommunityRepository, mapper, logger, options, emailService, userRepository, showingTimeContactsProvider)
        {
        }

        public Task<IEnumerable<LotListing>> GetPublicActiveListingsFromCommunity(CommunitySale community)
            => this.GetActiveListingsFromCommunity(community);
    }
}
