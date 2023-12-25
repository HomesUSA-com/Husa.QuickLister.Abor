namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Quickbooks.Models.Invoice;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Reports;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("bills")]
    public class BillsController : Controller
    {
        private readonly ISaleListingRequestQueriesRepository saleListingRequestRepository;
        private readonly ILogger<BillsController> logger;
        private readonly ISaleListingBillService saleListingBillService;
        private readonly IMapper mapper;
        public BillsController(
            ISaleListingRequestQueriesRepository saleListingRequestRepository,
            ILogger<BillsController> logger,
            ISaleListingBillService saleListingBillService,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.saleListingRequestRepository = saleListingRequestRepository ?? throw new ArgumentNullException(nameof(saleListingRequestRepository));
            this.saleListingBillService = saleListingBillService ?? throw new ArgumentNullException(nameof(saleListingBillService));
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

        [HttpPost]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> CreateInvoice(InvoiceRequest invoiceRequest)
        {
            this.logger.LogInformation("Starting to create invoice for company: {comapany}", invoiceRequest.CompanyId);
            var invoiceDto = this.mapper.Map<InvoiceDto>(invoiceRequest);
            var result = await this.saleListingBillService.ProcessInvoice(invoiceDto);
            if (result.Code == ResponseCode.Error)
            {
                return this.BadRequest(result);
            }

            return this.Ok(result.Result);
        }
    }
}
