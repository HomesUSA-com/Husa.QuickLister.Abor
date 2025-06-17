namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.ListingRequests;

    public class LotListingRequestService :
        ExtensionsServices.ListingRequestService<
            CommunitySale,
            LotListing,
            LotListingRequest,
            ICommunitySaleRepository,
            ILotListingRepository,
            ILotListingRequestRepository,
            ShowingTimeContact,
            IProvideShowingTimeContacts>,
        ILotListingRequestService
    {
        private readonly ApplicationOptions options;

        public LotListingRequestService(
            ILotListingRequestRepository saleRequestRepository,
            ILotListingRepository listingRepository,
            ILotListingRequestMediaService mediaService,
            IUserContextProvider userContextProvider,
            ICommunitySaleRepository saleCommunityRepository,
            IMapper mapper,
            ILogger<LotListingRequestService> logger,
            IOptions<ApplicationOptions> options,
            IEmailService emailService,
            IUserRepository userRepository,
            IProvideShowingTimeContacts showingTimeContactsProvider)
            : base(
                  saleRequestRepository,
                  mediaService,
                  userContextProvider,
                  saleCommunityRepository,
                  listingRepository,
                  emailService,
                  mapper,
                  logger,
                  userRepository,
                  showingTimeContactsProvider)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override int MinRequiredMedia => this.options.ListingRequest.MinRequiredMedia;
        protected override int MaxAllowedMedia => this.options.ListingRequest.MaxAllowedMedia;

        public async Task<LotListingRequest> UpdateRequestAsync(LotListingRequest request, LotListingRequestDto listingRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service starting to update request for ABOR listing sale with Id {requestId}", request.Id);
            var listingRequestValueObject = this.Mapper.Map<ListingRequestValueObject>(listingRequestDto);
            var statusFieldInfo = this.Mapper.Map<ListingStatusFieldsInfo>(listingRequestDto.StatusFieldsInfo);
            var propertyInfo = this.Mapper.Map<LotPropertyValueObject>(listingRequestDto);

            request.UpdateRequestInformation(listingRequestValueObject, statusFieldInfo, propertyInfo);
            var userId = this.UserContextProvider.GetCurrentUserId();

            await this.RequestRepository.UpdateDocumentAsync(request.Id, request, userId, cancellationToken);
            this.Logger.LogInformation("Request update was completed for ABOR listing request with Id {requestId}", request.Id);
            return request;
        }

        public async Task UpdateListingRequestAsync(Guid listingRequestId, LotListingRequestDto listingRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service is starting to update request for ABOR with id {listingRequestId}", listingRequestId);
            var listingRequest = await this.RequestRepository.GetByIdAsync(listingRequestId, cancellationToken);
            await this.UpdateRequestAsync(listingRequest, listingRequestDto, cancellationToken);
        }

        protected override Task SendReturnedListingRequestEmail(LotListingRequest request, string reason)
            => this.EmailService.SendReturnedListingRequestEmail(request, reason, "lot-listings");

        protected override IEnumerable<LotListing> GetActiveListingsFromCommunity(CommunitySale community)
            => community.GetActiveLotListings();
    }
}
