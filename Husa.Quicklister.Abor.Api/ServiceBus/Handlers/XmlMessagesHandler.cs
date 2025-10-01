namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Husa.Quicklister.Extensions.Application.Interfaces.XmlCommon;
    using Husa.Xml.ServiceBus.Messages;
    using Husa.Xml.ServiceBus.Messages.Messages;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;

    public class XmlMessagesHandler : MessagesHandler<XmlMessagesHandler>, IXmlMessagesHandler
    {
        private readonly XmlUserSettings userOptions;
        private readonly ApplicationOptions options;

        public XmlMessagesHandler(
            IXmlSubscriber xmlSubscriber,
            IServiceScopeFactory serviceProvider,
            IOptions<XmlUserSettings> userOptions,
            IOptions<ApplicationOptions> options,
            ILogger<XmlMessagesHandler> logger)
            : base(xmlSubscriber, serviceProvider, logger)
        {
            this.userOptions = userOptions is null ? throw new ArgumentNullException(nameof(userOptions)) : userOptions.Value;
            this.options = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        public override async Task ProcessMessageAsync(Message message, IServiceScope scope, CancellationToken cancellationToken)
        {
            ConfigureUserAgent();
            ConfigureContext();
            this.Logger.LogDebug("Deserializing message {messageId}.", message.MessageId);
            var receivedMessage = message.DeserializeMessage();
            switch (receivedMessage)
            {
                case ImportSubdivisionMessage importSubdivision:
                    await ImportEntity<ICommunityXmlService>(importSubdivision);
                    break;
                case ImportPlanMessage importPlan:
                    await ImportEntity<IPlanXmlService>(importPlan);
                    break;
                case UpdateSelfApproveSubdivisionMessage updateSubdivision:
                    await UpdateSelfApproveEntity<ICommunityXmlService>(updateSubdivision);
                    break;
                case UpdateSelfApprovePlanMessage updatePlan:
                    await UpdateSelfApproveEntity<IPlanXmlService>(updatePlan);
                    break;
                case ImportListingMessage xmlUpdateMessage:
                    await ProcessXmlUpdateMessage(xmlUpdateMessage);
                    break;
                case AutoMatchListingMessage autoMatchListingMessage:
                    await ProcessAutoMatchListingMessage(autoMatchListingMessage);
                    break;
                case ImportListingMedia importListingMedia:
                    await ImportListingMedia(importListingMedia);
                    break;
                case ImportPlanMedia importPlanMedia:
                    await ImportPlanMedia(importPlanMedia);
                    break;
                case ImportSubdivisionMedia importSubdivisionMedia:
                    await ImportSubdivisionMedia(importSubdivisionMedia);
                    break;
                default:
                    this.Logger.LogWarning("Message type not recognized for message {messageId}.", message.MessageId);
                    break;
            }

            Task ProcessXmlUpdateMessage(ImportListingMessage xmlUpdateMessage)
            {
                this.Logger.LogInformation("Processing Update message from Xml to listing with id {listingId} and companyId {companyId}", xmlUpdateMessage.Id, xmlUpdateMessage.CompanyId);
                var saleListingXmlService = scope.ServiceProvider.GetRequiredService<ISaleListingXmlService>();
                return saleListingXmlService.UpdateListingFromXmlAsync(xmlUpdateMessage.Id);
            }

            Task ProcessAutoMatchListingMessage(AutoMatchListingMessage automaticMatchingListingMessage)
            {
                this.Logger.LogInformation("Processing message from Xml to matching listing with id {listingId} and companyId {companyId}", automaticMatchingListingMessage.Id, automaticMatchingListingMessage.CompanyId);
                var saleListingXmlService = scope.ServiceProvider.GetRequiredService<ISaleListingXmlService>();
                return saleListingXmlService.AutoMatchListingFromXmlAsync(automaticMatchingListingMessage.Id);
            }

            Task ImportPlanMedia(ImportPlanMedia importPlanMedia)
            {
                this.Logger.LogInformation("Importing the media for the XML plan {planId} and companyId {companyId}", importPlanMedia.Id, importPlanMedia.CompanyId);
                var mediaImportService = scope.ServiceProvider.GetRequiredService<IPlanXmlMediaService>();
                return mediaImportService.ImportPlanMedia(importPlanMedia.Id, maxImagesAllowed: this.options.MediaAllowed.PlanMaxAllowedMedia);
            }

            Task ImportListingMedia(ImportListingMedia importListingMedia)
            {
                this.Logger.LogInformation("Importing the media for the XML listing {listingId} and companyId {companyId}", importListingMedia.Id, importListingMedia.CompanyId);
                var mediaImportService = scope.ServiceProvider.GetRequiredService<ISaleListingXmlMediaService>();
                return mediaImportService.ImportListingMedia(importListingMedia.Id);
            }

            Task ImportSubdivisionMedia(ImportSubdivisionMedia importSubdivisionMedia)
            {
                this.Logger.LogInformation("Importing the media for the XML subdivision {subdivisionId} and companyId {companyId}", importSubdivisionMedia.Id, importSubdivisionMedia.CompanyId);
                var mediaImportService = scope.ServiceProvider.GetRequiredService<ICommunityXmlMediaService>();
                return mediaImportService.ImportSubdivisionMedia(importSubdivisionMedia.Id, maxImagesAllowed: this.options.MediaAllowed.CommunityMaxAllowedMedia);
            }

            async Task ImportEntity<TService>(ImportProfileMessage message)
                where TService : IXmlCommon
            {
                if (message.MarketCode != MarketCode.Austin)
                {
                    return;
                }

                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await service.ImportEntity(message.CompanyId, message.CompanyName, message.Id, message.SelfApprove);
            }

            Task UpdateSelfApproveEntity<TService>(ImportProfileMessage message)
                where TService : IXmlCommon
            {
                if (message.MarketCode != MarketCode.Austin)
                {
                    return Task.CompletedTask;
                }

                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return service.UpdateSelfApproveEntity(message.CompanyId, message.CompanyName, message.Id);
            }

            UserContext GetXmlUser() => new()
            {
                Email = this.userOptions.Email,
                Name = this.userOptions.Name,
                Id = this.userOptions.Id,
                IsMLSAdministrator = this.userOptions.MLSAdministrator,
            };

            void ConfigureContext()
            {
                var userProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();
                userProvider.SetCurrentUser(GetXmlUser());
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
        }
    }
}
