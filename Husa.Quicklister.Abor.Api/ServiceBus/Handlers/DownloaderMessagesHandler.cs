namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;
    using DownloaderConstructionStage = Husa.Downloader.Sabor.Domain.Enums.ConstructionStage;

    public class DownloaderMessagesHandler : MessagesHandler<DownloaderMessagesHandler>, IDownloaderMessagesHandler
    {
        private readonly DownloaderUserSettings downloaderUserOptions;
        private readonly IMapper mapper;

        public DownloaderMessagesHandler(
            IDownloaderSubscriber downloaderSubscriber,
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IOptions<DownloaderUserSettings> options,
            ILogger<DownloaderMessagesHandler> logger)
            : base(downloaderSubscriber, scopeFactory, logger)
        {
            this.downloaderUserOptions = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task ProcessMessageAsync(Message message, IServiceScope scope, CancellationToken cancellationToken)
        {
            ConfigureUserAgent();
            ConfigureContext();
            this.Logger.LogDebug("Deserializing message {messageId}.", message.MessageId);
            var receivedMessage = message.DeserializeMessage();
            switch (receivedMessage)
            {
                case AgentMessage agentMessage:
                    await ProcessAgentMessage(agentMessage);
                    break;
                case OfficeMessage officeMessage:
                    await ProcessOfficeMessage(officeMessage);
                    break;
                case ResidentialMessage residentialMessage:
                    await ProcessResidentialMessage(residentialMessage);
                    break;
                case ResidentialOpenHousesMessage openHousesMessage:
                    await ProcessResidentialOpenHouseMessage(openHousesMessage);
                    break;
                case ResidentialMediaMessage mediaMessage:
                    await ProcessResidentialMediaMessage(mediaMessage);
                    break;
                default:
                    this.Logger.LogWarning("Message type not recognized for message {messageId}.", message.MessageId);
                    break;
            }

            Task ProcessAgentMessage(AgentMessage agentMessage)
            {
                this.Logger.LogInformation("Processing message for agent {agentLoginName} with id {agentId} ", agentMessage.LoginName, agentMessage.AgentId);
                var agentDto = this.mapper.Map<AgentDto>(agentMessage);

                var agentService = scope.ServiceProvider.GetRequiredService<IAgentService>();
                return agentService.ProcessDataFromDownloaderAsync(agentDto);
            }

            Task ProcessOfficeMessage(OfficeMessage officeMessage)
            {
                this.Logger.LogInformation("Processing message for office {officeName} with id {officeId}", officeMessage.Name, officeMessage.OfficeId);
                var officeService = scope.ServiceProvider.GetRequiredService<IOfficeService>();
                var officeDto = this.mapper.Map<OfficeDto>(officeMessage);
                return officeService.ProcessDataFromDownloaderAsync(officeDto);
            }

            Task ProcessResidentialMessage(ResidentialMessage residentialMessage)
            {
                this.Logger.LogInformation("Processing message for listing with mls number {mlsNumber}", residentialMessage.ResidentialValue.MlsNumber);
                var listingSaleDto = this.mapper.Map<FullListingSaleDto>(residentialMessage.ResidentialValue);
                var roomsDto = this.mapper.Map<IEnumerable<RoomDto>>(residentialMessage.Rooms);
                var hoasDto = this.mapper.Map<IEnumerable<HoaDto>>(residentialMessage.Hoas);

                SetLegacyInfo(listingSaleDto.SaleProperty, residentialMessage.LegacyResidentialInfo);
                var sellingAgent = residentialMessage.ResidentialValue.SellingAgent;

                var downloaderService = scope.ServiceProvider.GetRequiredService<IDownloaderService>();
                return downloaderService.ProcessDataFromDownloaderAsync(listingSaleDto, roomsDto, hoasDto, sellingAgent);
            }

            Task ProcessResidentialOpenHouseMessage(ResidentialOpenHousesMessage openHousesMessage)
            {
                this.Logger.LogInformation("Processing open houses for listing with mls number {mlsNumber}", openHousesMessage.MlsNumber);
                var openHousesDto = this.mapper.Map<IEnumerable<OpenHouseDto>>(openHousesMessage.OpenHouses);
                var downloaderService = scope.ServiceProvider.GetRequiredService<IDownloaderService>();
                return downloaderService.ProcessOpenHouseFromDownloaderAsync(openHousesMessage.MlsNumber, openHousesDto);
            }

            Task ProcessResidentialMediaMessage(ResidentialMediaMessage mediaMessage)
            {
                this.Logger.LogInformation("Processing media for listing with mls number {mlsNumber}", mediaMessage.MlsNumber);
                var mediaDto = this.mapper.Map<IEnumerable<ListingSaleMediaDto>>(mediaMessage.Media);
                var downloaderService = scope.ServiceProvider.GetRequiredService<IDownloaderService>();
                return downloaderService.ProcessMediaFromDownloaderAsync(mediaMessage.MlsNumber, mediaDto);
            }

            static ConstructionStage GetConstructionStage(DownloaderConstructionStage constructionStage) => constructionStage switch
            {
                DownloaderConstructionStage.Complete => ConstructionStage.Complete,
                DownloaderConstructionStage.Incomplete => ConstructionStage.Incomplete,
                _ => throw new ArgumentException($"The construction stage is invalid: {constructionStage}", nameof(constructionStage)),
            };

            static void SetLegacyInfo(SalePropertyDetailDto saleProperty, LegacyResidentialMessage legacyResidentialInfo)
            {
                saleProperty.PropertyInfo.ConstructionCompletionDate = legacyResidentialInfo.ConstructionCompletionDate;
                saleProperty.PropertyInfo.ConstructionStage = GetConstructionStage(legacyResidentialInfo.ConstructionStage);
                saleProperty.ShowingInfo.RealtorContactEmail = legacyResidentialInfo.EmailForRealtors;
                saleProperty.ShowingInfo.OccupantPhone = legacyResidentialInfo.OwnerAlternatePhone;
            }

            UserContext GetDownloaderUser() => new()
            {
                Email = this.downloaderUserOptions.Email,
                Name = this.downloaderUserOptions.Name,
                Id = this.downloaderUserOptions.Id,
                IsMLSAdministrator = this.downloaderUserOptions.MLSAdministrator,
            };

            void ConfigureContext()
            {
                var userProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();
                userProvider.SetCurrentUser(GetDownloaderUser());
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
