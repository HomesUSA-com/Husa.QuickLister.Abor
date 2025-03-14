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
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("bills")]
    public class BillsController : Controller
    {
        private readonly IListingRequestBillingQueryRepository billingRequestRepository;
        private readonly ISaleListingBillService saleListingBillService;
        private readonly ILogger<BillsController> logger;
        private readonly IMapper mapper;
        public BillsController(
            IListingRequestBillingQueryRepository saleListingRequestRepository,
            ISaleListingBillService saleListingBillService,
            ILogger<BillsController> logger,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.billingRequestRepository = saleListingRequestRepository ?? throw new ArgumentNullException(nameof(saleListingRequestRepository));
            this.saleListingBillService = saleListingBillService ?? throw new ArgumentNullException(nameof(saleListingBillService));
        }

        [HttpGet]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> GetBillingListing([FromQuery] ListingSaleBillingRequestFilter filters)
        {
            this.logger.LogInformation("Starting to get the billable listing");
            var requestFilter = this.mapper.Map<ListingBillingQueryFilter>(filters);
            var listing = await this.billingRequestRepository.GetBillableListingsAsync(requestFilter);
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
