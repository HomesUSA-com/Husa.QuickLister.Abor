namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.EmailNotification.Enums;
    using Husa.Extensions.EmailNotification.Services;
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
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IEmailSender emailSender;

        public LotListingRequestService(
            ILotListingRequestRepository saleRequestRepository,
            ILotListingRepository listingRepository,
            ILotListingRequestMediaService mediaService,
            IUserContextProvider userContextProvider,
            ICommunitySaleRepository saleCommunityRepository,
            IMapper mapper,
            ILogger<LotListingRequestService> logger,
            IOptions<ApplicationOptions> options,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IEmailSender emailSender,
            IUserRepository userRepository,
            IProvideShowingTimeContacts showingTimeContactsProvider)
            : base(
                  saleRequestRepository,
                  mediaService,
                  userContextProvider,
                  saleCommunityRepository,
                  listingRepository,
                  mapper,
                  logger,
                  userRepository,
                  showingTimeContactsProvider)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
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

        protected override async Task SendReturnedListingRequestEmail(LotListingRequest request, string reason)
        {
            var userId = new Guid(request.SysCreatedBy.ToString());

            var user = await this.serviceSubscriptionClient.User.GetUserDetail(userId);

            var callbackUrl = $"{this.options.QuicklisterUIUri}/lot-listings/{request.EntityId}";

            var emailParameter = new Dictionary<EmailParameter, string>
            {
                { EmailParameter.Link, callbackUrl },
                { EmailParameter.Name, user.FirstName },
                { EmailParameter.Address, request.AddressInfo.FormalAddress },
                { EmailParameter.ReturnedReason, reason },
            };

            this.emailSender.SendEmail(user.Email, user.FirstName, emailParameter, TemplateType.ReturnedListingRequest);
        }

        protected override IEnumerable<LotListing> GetActiveListingsFromCommunity(CommunitySale community)
            => community.GetActiveLotListings();
    }
}
