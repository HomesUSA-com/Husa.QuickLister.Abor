namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.ListingRequests;

    public class SaleListingRequestService :
        ExtensionsServices.SaleListingRequestService<
            CommunitySale,
            SaleListing,
            SaleListingRequest,
            ICommunitySaleRepository,
            IListingSaleRepository,
            ISaleListingRequestRepository>,
        ISaleListingRequestService
    {
        private readonly ApplicationOptions options;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IEmailSender emailSender;

        public SaleListingRequestService(
            ISaleListingRequestRepository saleRequestRepository,
            IListingSaleRepository listingSaleRepository,
            ExtensionsInterfaces.IListingRequestMediaService mediaService,
            IUserContextProvider userContextProvider,
            ICommunitySaleRepository saleCommunityRepository,
            IMapper mapper,
            ILogger<SaleListingRequestService> logger,
            IOptions<ApplicationOptions> options,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IEmailSender emailSender,
            IUserRepository userRepository)
            : base(
                  saleRequestRepository,
                  mediaService,
                  userContextProvider,
                  saleCommunityRepository,
                  listingSaleRepository,
                  mapper,
                  logger,
                  userRepository)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<SaleListingRequest> UpdateRequestAsync(SaleListingRequest request, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service starting to update request for ABOR listing sale with Id {requestId}", request.Id);
            var saleProperty = this.Mapper.Map<SaleProperty>(listingSaleRequestDto.SaleProperty);
            request.UpdateRequestInformation(listingSaleRequestDto.ListPrice, saleProperty);
            var userId = this.UserContextProvider.GetCurrentUserId();
            await this.SaleRequestRepository.UpdateDocumentAsync(request.Id, request, userId, cancellationToken);
            this.Logger.LogInformation("Request update was completed for ABOR listing request with Id {requestId}", request.Id);
            return request;
        }

        public async Task UpdateListingRequestAsync(Guid listingRequestId, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service is starting to update request for ABOR with id {listingRequestId}", listingRequestId);
            var listingRequestValueObject = this.Mapper.Map<ListingRequestValueObject>(listingSaleRequestDto);
            var statusFieldInfo = this.Mapper.Map<ListingSaleStatusFieldsInfo>(listingSaleRequestDto.StatusFieldsInfo);
            var salePropertyInfo = this.Mapper.Map<SalePropertyValueObject>(listingSaleRequestDto.SaleProperty);

            var listingRequest = await this.SaleRequestRepository.GetByIdAsync(listingRequestId, cancellationToken);
            listingRequest.UpdateRequestInformation(listingRequestValueObject, statusFieldInfo, salePropertyInfo);
            var userId = this.UserContextProvider.GetCurrentUserId();

            await this.SaleRequestRepository.UpdateDocumentAsync(listingRequestId, listingRequest, userId, cancellationToken);
        }

        protected override async Task<string> IsImageCountValidAsync(Guid saleListingId)
        {
            var mediaCount = (await this.MediaService.GetListingResources(saleListingId)).Media.Count();

            if (mediaCount < this.options.ListingRequest.MinRequiredMedia)
            {
                return $"{this.options.ListingRequest.MinRequiredMedia} photos are required to submit this listing";
            }

            if (mediaCount > this.options.ListingRequest.MaxAllowedMedia)
            {
                return $"No more than {this.options.ListingRequest.MaxAllowedMedia} photos are allowed to submit this listing";
            }

            return string.Empty;
        }

        protected override async Task SendReturnedListingRequestEmail(SaleListingRequest request, string reason)
        {
            var userId = new Guid(request.SysCreatedBy.ToString());

            var user = await this.serviceSubscriptionClient.User.GetUserDetail(userId);

            var callbackUrl = $"{this.options.QuicklisterUIUri}/listings/sale/{request.ListingSaleId}";

            var emailParameter = new Dictionary<EmailParameter, string>
            {
                { EmailParameter.Link, callbackUrl },
                { EmailParameter.Name, user.FirstName },
                { EmailParameter.Address, request.SaleProperty.Address },
                { EmailParameter.ReturnedReason, reason },
            };

            this.emailSender.SendEmail(user.Email, user.FirstName, emailParameter, TemplateType.ReturnedListingRequest);
        }
    }
}
