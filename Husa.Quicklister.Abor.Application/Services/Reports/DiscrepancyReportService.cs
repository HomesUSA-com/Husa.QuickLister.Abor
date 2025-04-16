namespace Husa.Quicklister.Abor.Application.Services.Reports
{
    using AutoMapper;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.Sabor.Client;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Reports;
    using Microsoft.Extensions.Logging;
    using QLExtensions = Husa.Quicklister.Extensions.Application.Services.Reports;
    public class DiscrepancyReportService : QLExtensions.DiscrepancyReportService<SaleListing, IListingSaleRepository>, IDiscrepancyReportService
    {
        public DiscrepancyReportService(
            IListingSaleRepository listingRepository,
            IUserContextProvider userContextProvider,
            IXmlClientWithoutToken xmlClient,
            IDownloaderCtxClient downloaderCtxClient,
            IDownloaderSaborClient downloaderSaborClient,
            IMapper mapper,
            ILogger<DiscrepancyReportService> logger)
            : base(listingRepository, userContextProvider, xmlClient, downloaderCtxClient, downloaderSaborClient, mapper, logger)
        {
        }
    }
}
