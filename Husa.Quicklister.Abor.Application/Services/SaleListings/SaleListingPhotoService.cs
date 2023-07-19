namespace Husa.Quicklister.Abor.Application.Services.SaleListings
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
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    public class SaleListingPhotoService : PhotoService<IListingSaleRepository, SaleListing>, ISaleListingPhotoService
    {
        public SaleListingPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IListingSaleRepository listingSaleRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<SaleListingPhotoService> logger)
         : base(
               serviceBusSettings,
               photoClient,
               userContextProvider,
               busClient,
               traceIdProvider,
               listingSaleRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override PropertyType PropertyType => PropertyType.Residential;

        public async override Task AssignLatestPhotoRequest(Guid entityId, Guid photorequestId, DateTime creationDate)
        {
            var entity = await this.EntityRepository.GetById(entityId, filterByCompany: false) ?? throw new NotFoundException<SaleListing>(entityId);
            entity.SetLastPhotoRequestInfo(photorequestId, creationDate);
            await this.EntityRepository.SaveChangesAsync(entity);
        }

        public async override Task<PhotoRequestBusMessage> GetPhotoRequestMessage(Guid entityId, Request.PhotoRequest photoRequest)
        {
            var entity = await this.EntityRepository.GetById(entityId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(entityId);
            var message = await base.GetPhotoRequestMessage(entityId, photoRequest);
            message.Property.PlanName = entity.SaleProperty.PlanId.HasValue ? entity.SaleProperty.Plan.BasePlan.Name : null;
            return message;
        }
    }
}
