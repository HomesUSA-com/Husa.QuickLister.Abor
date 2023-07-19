namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response.Reports;

    public interface IReport
    {
        Task<IEnumerable<Response.ScrapedListingQueryResponse>> GetComparisonReportAsync(ScrapedListingRequestFilter filter, CancellationToken token = default);
    }
}
