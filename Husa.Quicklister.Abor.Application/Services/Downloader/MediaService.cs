namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MediaService : IMediaService
    {
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ISaleListingMediaService listingMediaService;
        private readonly IDownloaderCtxClient downloaderCtxClient;
        private readonly ApplicationOptions options;
        private readonly ILogger<MediaService> logger;

        public MediaService(
            IListingSaleRepository listingSaleRepository,
            ISaleListingMediaService saleListingMediaService,
            IDownloaderCtxClient downloaderCtxClient,
            IOptions<ApplicationOptions> options,
            IMapper mapper,
            ILogger<MediaService> logger)
        {
            this.listingMediaService = saleListingMediaService ?? throw new ArgumentNullException(nameof(saleListingMediaService));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.downloaderCtxClient = downloaderCtxClient ?? throw new ArgumentNullException(nameof(downloaderCtxClient));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessData(Guid listingId, DateTime updatedOn)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            this.logger.LogInformation("Starting to process media from downloader for listing sale with id {listingId} and Mls number {mlsNumber}", listingId, listingSale.MlsNumber);
            var listingMedia = await this.downloaderCtxClient.Residential.GetResidentialMediaByIdAsync(listingId, updatedOn);
            var mediaDto = this.mapper.Map<IEnumerable<ListingSaleMediaDto>>(listingMedia);

            if (!mediaDto.Any())
            {
                this.logger.LogInformation("No media was found in the MLS to import for listing id {listingId}", listingId);
                return;
            }

            await this.listingMediaService.Resource.DeleteAsync(listingSale.Id, dispose: false);
            await this.listingMediaService.Resource.BulkCreateAsync(listingSale.Id, mediaDto, mediaLimitAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia);
        }
    }
}
