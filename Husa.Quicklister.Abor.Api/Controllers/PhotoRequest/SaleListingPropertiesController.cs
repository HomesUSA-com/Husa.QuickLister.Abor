namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response.PhotoRequest;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listings/{listingId}/properties")]
    public class SaleListingPropertiesController : ControllerBase
    {
        private readonly IListingSaleQueriesRepository listingSaleQueriesRepository;
        private readonly IMapper mapper;
        private readonly ILogger<SaleListingPropertiesController> logger;
        public SaleListingPropertiesController(IListingSaleQueriesRepository listingSaleQueriesRepository, IMapper mapper, ILogger<SaleListingPropertiesController> logger)
        {
            this.listingSaleQueriesRepository = listingSaleQueriesRepository ?? throw new ArgumentNullException(nameof(listingSaleQueriesRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Obsolete("Avoid using this endpoint it is legacy and will be removed in the future, only for app support")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid listingId, CancellationToken cancellationToken = default)
        {
            var propertyResponse = await this.GetPropertyByIdAsync(listingId, propertyId: listingId, cancellationToken);
            return this.Ok(propertyResponse);
        }

        [HttpGet("{propertyId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid listingId, [FromRoute] Guid propertyId, CancellationToken cancellationToken = default)
        {
            var propertyResponse = await this.GetPropertyByIdAsync(listingId, propertyId, cancellationToken);
            return this.Ok(propertyResponse);
        }

        private async Task<PhotoRequestPropertyResponse> GetPropertyByIdAsync(Guid listingId, Guid propertyId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to GET property for listing {listingId}.", listingId);
            var property = await this.listingSaleQueriesRepository.GetListingPhotoProperty(listingId, propertyId, cancellationToken);
            return this.mapper.Map<PhotoRequestPropertyResponse>(property);
        }
    }
}
