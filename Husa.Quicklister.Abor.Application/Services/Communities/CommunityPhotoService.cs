namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    public class CommunityPhotoService : PhotoService<ICommunitySaleRepository, CommunitySale>, ICommunityPhotoService
    {
        public CommunityPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            ICommunitySaleRepository communitySaleRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CommunityPhotoService> logger)
         : base(
               serviceBusSettings,
               photoClient,
               userContextProvider,
               busClient,
               traceIdProvider,
               communitySaleRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override PropertyType PropertyType => PropertyType.Community;

        public async override Task AssignLatestPhotoRequest(Guid entityId, Guid photorequestId, DateTime creationDate)
        {
            var entity = await this.EntityRepository.GetById(entityId, filterByCompany: false) ?? throw new NotFoundException<CommunitySale>(entityId);
            entity.SetLastPhotoRequestInfo(photorequestId, creationDate);
            await this.EntityRepository.SaveChangesAsync(entity);
        }

        public async override Task<PhotoRequestBusMessage> GetPhotoRequestMessage(Guid entityId, Request.PhotoRequest photoRequest)
        {
            var message = await base.GetPhotoRequestMessage(entityId, photoRequest);
            var communityInfo = photoRequest.CommunityInfo ?? throw new ArgumentNullException(nameof(photoRequest), $"The value of {nameof(photoRequest.CommunityInfo)} in the photo request must be informed");

            message.CommunityInfo = new CommunityInfoBusMessage
            {
                CommunityServiceOption = communityInfo.CommunityServiceOption,
                DigitalTwilight = communityInfo.DigitalTwilight,
                ConfirmCommunityAccessible = communityInfo.ConfirmCommunityAccessible,
            };

            return message;
        }
    }
}
