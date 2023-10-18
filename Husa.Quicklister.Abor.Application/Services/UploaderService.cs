namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.ReverseProspect.Api.Client;
    using Husa.ReverseProspect.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class UploaderService : IUploaderService
    {
        private readonly ILogger<UploaderService> logger;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly IReverseProspectRepository reverseProspectRepository;
        private readonly IReverseProspectClient reverseProspectClient;
        private readonly IUserContextProvider userContextProvider;

        public UploaderService(
            IListingSaleRepository listingSaleRepository,
            IReverseProspectRepository reverseProspectRepository,
            IReverseProspectClient reverseProspectClient,
            IUserContextProvider userContextProvider,
            ILogger<UploaderService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.reverseProspectRepository = reverseProspectRepository ?? throw new ArgumentNullException(nameof(reverseProspectRepository));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.reverseProspectClient = reverseProspectClient ?? throw new ArgumentNullException(nameof(reverseProspectClient));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
        }

        public async Task<CommandResult<ReverseProspectData>> GetReverseProspectListing(Guid listingId, bool usingDatabase = true, CancellationToken cancellationToken = default)
        {
            var userId = this.userContextProvider.GetCurrentUserId();
            var saleListing = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, userId, saleListing.CompanyId, null, ReverseProspectStatus.Requested));

            var reverseProspect = usingDatabase ? await this.reverseProspectRepository.GetReverseProspectByTrackingId(listingId) : null;
            if (reverseProspect != null && reverseProspect.HasReportData && reverseProspect.Status == ReverseProspectStatus.Available)
            {
                this.logger.LogInformation("Reverse prospect found for listing with id {listingId} from tracking table.", listingId);
                return CommandResult<ReverseProspectData>.Success(JsonConvert.DeserializeObject<IEnumerable<ReverseProspectData>>(reverseProspect.ReportData));
            }

            var uploadResult = await this.reverseProspectClient.ReverseProspectRequest.GetReverseProspectData(saleListing.MlsNumber, MarketCode.Austin);
            if (uploadResult.Code == ResponseCode.Error)
            {
                this.logger.LogInformation("Saving Reverse prospect data response error for listing {listingId} in tracking table.", listingId);
                await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, userId, saleListing.CompanyId, null, ReverseProspectStatus.NotAvailable));
                return CommandResult<ReverseProspectData>.Error($"Reverse prospect data response error for listing with id {listingId} from market proccess");
            }

            this.logger.LogInformation("Reverse prospect data response found for listing {listingId} from chromedriver.", listingId);
            await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, userId, saleListing.CompanyId, JsonConvert.SerializeObject(uploadResult.Results), ReverseProspectStatus.Available));
            return CommandResult<ReverseProspectData>.Success(uploadResult.Results);
        }
    }
}
