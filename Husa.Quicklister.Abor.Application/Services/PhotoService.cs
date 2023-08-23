namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Client.Resources;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Domain.Entities;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Application.Interfaces.PhotoRequest;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Request = Husa.PhotoService.Api.Contracts.Request;
    using Response = Husa.PhotoService.Api.Contracts.Response;

    public abstract class PhotoService<TEntityRepository, TEntity> : MessagingServiceBusBase, IPhotoService
        where TEntity : Entity
        where TEntityRepository : IRepository<TEntity>
    {
        private readonly IPhotoServiceClient photoClient;
        private readonly IProvideTraceId traceIdProvider;
        private readonly IUserContextProvider userContextProvider;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        protected PhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IPhotoServiceClient photoClient,
            IUserContextProvider userContextProvider,
            ServiceBusClient client,
            IProvideTraceId traceIdProvider,
            TEntityRepository entityRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger logger)
            : base(logger, client, serviceBusSettings.Value.PhotoService.TopicName)
        {
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.photoClient = photoClient ?? throw new ArgumentNullException(nameof(photoClient));
            this.traceIdProvider = traceIdProvider ?? throw new ArgumentNullException(nameof(traceIdProvider));
            this.EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public TEntityRepository EntityRepository { get; }

        public abstract PropertyType PropertyType { get; }

        protected ILogger Logger { get; }

        public abstract Task AssignLatestPhotoRequest(Guid entityId, Guid photorequestId, DateTime creationDate);

        public async Task CreateAsync(Guid entityId, Request.PhotoRequest photoRequest)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            var company = await this.serviceSubscriptionClient.Company.GetCompany(photoRequest.CompanyId) ?? throw new NotFoundException<Company>(photoRequest.CompanyId);

            if (!company.PhotographyServiceInfo.PhotographyServicesEnabled)
            {
                throw new DomainException($"The photography service is not enabled for company with id: {photoRequest.CompanyId}");
            }

            var photoRequestBusMessage = await this.GetPhotoRequestMessage(entityId, photoRequest);
            var photoMessage = new CreatePhotoRequestMessage
            {
                PhotoRequest = photoRequestBusMessage,
            };
            await this.SendBusMessage(photoMessage);
        }

        public async Task<Response.PhotoRequestDetail> GetByIdAsync(Guid entityId, Guid photoRequestId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.Logger.LogInformation("Starting to GET a photoRequest by id: {photoRequestId}.", photoRequestId);
            return await this.photoClient.PhotoRequest.GetByIdAsync(photoRequestId);
        }

        public async Task<DataSet<Response.PhotoRequestResponse>> GetAsync(Guid entityId, Request.PhotoRequestFilter filters)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.Logger.LogInformation("Getting active photorequests for entity {entityId} of type {propertyType}", entityId, this.PropertyType);
            filters.Type = new[] { this.PropertyType };
            filters.PropertyIds = new List<Guid> { entityId };
            return await this.photoClient.PhotoRequest.GetAsync(filters);
        }

        public async Task DeleteByIdAsync(Guid entityId, Guid photorequestId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            var photoMessage = new DeletePhotoRequestMessage
            {
                Id = photorequestId,
                Message = string.Empty,
            };
            await this.SendBusMessage(photoMessage);
        }

        public virtual Task<PhotoRequestBusMessage> GetPhotoRequestMessage(Guid entityId, Request.PhotoRequest photoRequest)
        {
            if (photoRequest is null)
            {
                throw new ArgumentNullException(nameof(photoRequest));
            }

            var property = photoRequest.Property ?? throw new ArgumentNullException(nameof(photoRequest), $"The value of {nameof(photoRequest.Property)} in the photo request must be informed");

            return Task.FromResult(new PhotoRequestBusMessage
            {
                Status = photoRequest.Status,
                ContactDate = photoRequest.ContactDate,
                ScheduleDate = photoRequest.ScheduleDate,
                CompanyId = photoRequest.CompanyId,
                CompanyName = photoRequest.CompanyName,
                Property = new PropertyBusMessage
                {
                    Id = entityId,
                    Type = this.PropertyType,
                    StreetNum = property.StreetNum,
                    StreetName = property.StreetName,
                    StreetType = property.StreetType,
                    StreetDirection = property.StreetDirection,
                    UnitNumber = property.UnitNumber,
                    City = property.City,
                    ReadableCity = property.ReadableCity,
                    Zip = property.Zip,
                    State = property.State,
                    Subdivision = property.Subdivision,
                    MarketCode = MarketCode.Austin,
                },
                SalesCounselor = photoRequest.SalesCounselor,
                Assistant = photoRequest.Assistant,
                Directions = photoRequest.Directions,
                OtherInstructions = photoRequest.OtherInstructions,
                SpecialNotes = photoRequest.SpecialNotes,
                KeyLocation = photoRequest.KeyLocation,
                PrimaryEmail = photoRequest.PrimaryEmail,
                CCEmails = photoRequest.SecondaryEmail.ToCollectionFromString(";"),
                AgreePhotoReady = photoRequest.AgreePhotoReady,
                ConfirmPhotoReady = photoRequest.ConfirmPhotoReady,
                JobName = photoRequest.JobName,
                ModelAddress = photoRequest.ModelAddress,
                Phones = photoRequest.Phones.Select(p => new PhoneBusMessage
                {
                    Code = p.Code,
                    Number = p.Number,
                }),
                ExteriorOptions = photoRequest.ExteriorOptions,
                InteriorOptions = photoRequest.InteriorOptions,
                ServiceOptions = photoRequest.ServiceOptions,
                CommunityInfo = new CommunityInfoBusMessage(),
            });
        }

        private async Task ValidateEntityAndUserCompany(Guid entityId)
        {
            var entity = await this.EntityRepository.GetById(entityId, filterByCompany: true) ?? throw new NotFoundException<TEntity>(entityId);
            var user = this.userContextProvider.GetCurrentUser();
            if (user.UserRole == UserRole.User && !user.CompanyId.Value.Equals(entity.CompanyId))
            {
                throw new DomainException($"The current user logged has not allow to get the photo request related to the {this.PropertyType} with id: {entityId}");
            }
        }

        private Task SendBusMessage<T>(T mediaMessage)
             where T : IProvideBusEvent
        {
            var userId = this.userContextProvider.GetCurrentUserId();
            return this.SendMessage(new[] { mediaMessage }, userId: userId.ToString(), correlationId: this.traceIdProvider.TraceId);
        }
    }
}
