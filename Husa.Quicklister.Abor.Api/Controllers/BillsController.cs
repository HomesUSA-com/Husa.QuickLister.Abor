namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("bills")]
    public class BillsController : Controller
    {
        private readonly ISaleListingRequestQueriesRepository saleListingRequestRepository;
        private readonly ILogger<SaleListingsController> logger;
        private readonly IMapper mapper;
        public BillsController(
            ISaleListingRequestQueriesRepository saleListingRequestRepository,
            ILogger<SaleListingsController> logger,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.saleListingRequestRepository = saleListingRequestRepository ?? throw new ArgumentNullException(nameof(saleListingRequestRepository));
        }

        [HttpGet]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> GetBillingListing([FromQuery] ListingSaleBillingRequestFilter filters)
        {
            this.logger.LogInformation("Starting to get the billable listing");
            var requestFilter = this.mapper.Map<ListingSaleBillingQueryFilter>(filters);
            var listing = await this.saleListingRequestRepository.GetBillableListingsAsync(requestFilter);
            return this.Ok(listing);
        }
    }
}
