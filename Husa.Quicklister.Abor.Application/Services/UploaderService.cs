namespace Husa.Quicklister.Abor.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Application.Models.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.ReverseProspect.Api.Client;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class UploaderService : IUploaderService
    {
        private readonly ILogger<UploaderService> logger;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly IReverseProspectRepository reverseProspectRepository;
        private readonly IReverseProspectClient reverseProspectClient;
        private readonly IUserContextProvider userContextProvider;
        private readonly IMapper mapper;

        public UploaderService(
            IListingSaleRepository listingSaleRepository,
            IReverseProspectRepository reverseProspectRepository,
            IReverseProspectClient reverseProspectClient,
            IUserContextProvider userContextProvider,
            ILogger<UploaderService> logger,
            IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.reverseProspectRepository = reverseProspectRepository ?? throw new ArgumentNullException(nameof(reverseProspectRepository));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.reverseProspectClient = reverseProspectClient ?? throw new ArgumentNullException(nameof(reverseProspectClient));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CommandResult<ReverseProspectInformationDto>> GetReverseProspectListing(Guid listingId, bool usingDatabase = true, CancellationToken cancellationToken = default)
        {
            var saleListing = await this.listingSaleRepository.GetById(listingId, filterByCompany: true);

            if (saleListing == null)
            {
                this.logger.LogInformation("Listing information with id {listingId} was not found", listingId);
                throw new NotFoundException<Domain.Entities.Listing.SaleListing>(listingId);
            }

            var currentUser = this.userContextProvider.GetCurrentUser();
            await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, currentUser.Id, saleListing.CompanyId, null, ReverseProspectStatus.Requested));

            var reverseProspect = usingDatabase ? await this.reverseProspectRepository.GetReverseProspectByTrackingId(listingId) : null;

            CommandResult<ReverseProspectInformationDto> reverseProspectResult;
            if (reverseProspect != null && reverseProspect.HasReportData && reverseProspect.Status == ReverseProspectStatus.Available)
            {
                this.logger.LogInformation("Reverse prospect found for listing with id {listingId} from track table.", listingId);
                var responseInfo = new ReverseProspectInformationDto
                {
                    ReverseProspectData = JsonConvert.DeserializeObject<IEnumerable<ReverseProspectDataDto>>(reverseProspect.ReportData),
                    RequestedDate = reverseProspect.SysTimestamp,
                };

                reverseProspectResult = CommandResult<ReverseProspectInformationDto>.Success(responseInfo);
            }
            else
            {
                var uploadResult = await this.reverseProspectClient.ReverseProspectRequest.GetReverseProspectData(MarketCode.Austin, saleListing.MlsNumber, saleListing.CompanyId, marketStatus: saleListing.MlsStatus.ToString(), cancellationToken);

                if (uploadResult.Code == ResponseCode.Error)
                {
                    this.logger.LogInformation("Reverse prospect data response error for listing with id {listingId} to save in track table.", listingId);
                    await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, currentUser.Id, saleListing.CompanyId, null, ReverseProspectStatus.NotAvailable));
                    return CommandResult<ReverseProspectInformationDto>.Error($"Reverse prospect data response error for listing with id {listingId} from market proccess");
                }

                this.logger.LogInformation("Reverse prospect data response found for listing with id {listingId} from chromedriver.", listingId);
                var reverseProspectData = this.mapper.Map<IEnumerable<ReverseProspectDataDto>>(uploadResult.Results);
                var responseInfo = new ReverseProspectInformationDto
                {
                    ReverseProspectData = reverseProspectData,
                    RequestedDate = DateTime.UtcNow,
                };
                await this.reverseProspectRepository.AddAsync(new ReverseProspect(listingId, currentUser.Id, saleListing.CompanyId, JsonConvert.SerializeObject(uploadResult.Results), ReverseProspectStatus.Available));
                reverseProspectResult = CommandResult<ReverseProspectInformationDto>.Success(responseInfo);
            }

            return reverseProspectResult;
        }
    }
}
