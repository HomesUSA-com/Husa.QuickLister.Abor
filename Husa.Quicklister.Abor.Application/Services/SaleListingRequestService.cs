namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
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

    public class SaleListingRequestService :
        ExtensionsServices.SaleListingRequestService<
            CommunitySale,
            SaleListing,
            SaleListingRequest,
            ICommunitySaleRepository,
            IListingSaleRepository,
            ISaleListingRequestRepository,
            ShowingTimeContact,
            IProvideShowingTimeContacts>,
        ISaleListingRequestService
    {
        private readonly ApplicationOptions options;

        public SaleListingRequestService(
            ISaleListingRequestRepository saleRequestRepository,
            IListingSaleRepository listingSaleRepository,
            ISaleListingRequestMediaService mediaService,
            IUserContextProvider userContextProvider,
            ICommunitySaleRepository saleCommunityRepository,
            IMapper mapper,
            ILogger<SaleListingRequestService> logger,
            IOptions<ApplicationOptions> options,
            IEmailService emailService,
            IUserRepository userRepository,
            IRequestErrorRepository requestErrorRepository,
            IProvideShowingTimeContacts showingTimeContactsProvider)
            : base(
                  saleRequestRepository,
                  mediaService,
                  userContextProvider,
                  saleCommunityRepository,
                  listingSaleRepository,
                  requestErrorRepository,
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

        public async Task<SaleListingRequest> UpdateRequestAsync(SaleListingRequest request, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service starting to update request for ABOR listing sale with Id {requestId}", request.Id);
            var saleProperty = this.Mapper.Map<SaleProperty>(listingSaleRequestDto.SaleProperty);
            request.UpdateRequestInformation(listingSaleRequestDto.ListPrice, saleProperty);
            var userId = this.UserContextProvider.GetCurrentUserId();
            await this.RequestRepository.UpdateDocumentAsync(request.Id, request, userId, cancellationToken);
            this.Logger.LogInformation("Request update was completed for ABOR listing request with Id {requestId}", request.Id);
            return request;
        }

        public async Task UpdateListingRequestAsync(Guid listingRequestId, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Service is starting to update request for ABOR with id {listingRequestId}", listingRequestId);
            var listingRequestValueObject = this.Mapper.Map<ListingRequestValueObject>(listingSaleRequestDto);
            var statusFieldInfo = this.Mapper.Map<ListingStatusFieldsInfo>(listingSaleRequestDto.StatusFieldsInfo);
            var salePropertyInfo = this.Mapper.Map<SalePropertyValueObject>(listingSaleRequestDto.SaleProperty);

            var listingRequest = await this.RequestRepository.GetByIdAsync(listingRequestId, cancellationToken);
            listingRequest.UpdateRequestInformation(listingRequestValueObject, statusFieldInfo, salePropertyInfo);
            await this.UpdateListingShowingTime(listingRequest, listingSaleRequestDto.ShowingTime);

            var userId = this.UserContextProvider.GetCurrentUserId();

            await this.RequestRepository.UpdateDocumentAsync(listingRequestId, listingRequest, userId, cancellationToken);
        }
    }
}
