namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Extensions.Application.Interfaces;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    public class PhotoServiceMessagesHandler : MessagesHandler<PhotoServiceMessagesHandler>, IPhotoServiceMessagesHandler
    {
        public PhotoServiceMessagesHandler(
            IPhotoServiceSubscriber photoRequestSubscriber,
            IServiceScopeFactory serviceProvider,
            ILogger<PhotoServiceMessagesHandler> logger)
            : base(photoRequestSubscriber, serviceProvider, logger)
        {
        }

        public override async Task ProcessMessageAsync(Message message, IServiceScope scope, CancellationToken cancellationToken)
        {
            ConfigureUserAgent();
            ConfigureContext();
            this.Logger.LogDebug("Deserializing message {messageId}.", message.MessageId);
            var receivedMessage = message.DeserializeMessage();
            switch (receivedMessage)
            {
                case PhotoRequestCreatedMessage photoCreated:
                    var photoService = ResolvePhotoService(scope, photoCreated.Type);
                    if (photoService != null)
                    {
                        this.Logger.LogInformation("Message notifying a new photo request was created");
                        await photoService.AssignLatestPhotoRequest(photoCreated.PropertyId, photoCreated.Id, photoCreated.SysCreatedOn);
                    }

                    break;
                default:
                    this.Logger.LogWarning("Message type not recognized for message {messageId}.", message.MessageId);
                    break;
            }

            void ConfigureContext()
            {
                var userProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();
                userProvider.SetCurrentUser(new UserContext
                {
                    Id = Guid.Parse(message.UserProperties["UserId"].ToString()),
                });
                var configureTraceId = scope.ServiceProvider.GetRequiredService<IConfigureTraceId>();
                configureTraceId.SetTraceId(message.MessageId);
            }

            void ConfigureUserAgent()
            {
                var headerPropagationValues = scope.ServiceProvider.GetRequiredService<HeaderPropagationValues>();
                headerPropagationValues.Headers = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase)
                {
                    { "User-Agent", "background-service" },
                };
            }

            static IPhotoService ResolvePhotoService(IServiceScope scope, PhotoRequestType photoRequestType) => photoRequestType switch
            {
                PhotoRequestType.Residential => scope.ServiceProvider.GetRequiredService<ISaleListingPhotoService>(),
                PhotoRequestType.Community => scope.ServiceProvider.GetRequiredService<ICommunityPhotoService>(),
                PhotoRequestType.Plan => scope.ServiceProvider.GetRequiredService<IPlanPhotoService>(),
                _ => null,
            };
        }
    }
}
