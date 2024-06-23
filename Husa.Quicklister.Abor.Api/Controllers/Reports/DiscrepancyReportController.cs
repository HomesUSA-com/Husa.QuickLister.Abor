namespace Husa.Quicklister.Abor.Api.Controllers.Reports
{
    using AutoMapper;
    using Husa.Quicklister.Extensions.Application.Interfaces.Reports;
    using Microsoft.Extensions.Logging;
    using ExtensionController = Husa.Quicklister.Extensions.Api.Controllers.Reports;
    public class DiscrepancyReportController : ExtensionController.DiscrepancyReportController<IDiscrepancyReportService>
    {
        public DiscrepancyReportController(
            IDiscrepancyReportService discrepancyReportService,
            ILogger<DiscrepancyReportController> logger,
            IMapper mapper)
            : base(discrepancyReportService, mapper, logger)
        {
        }
    }
}
