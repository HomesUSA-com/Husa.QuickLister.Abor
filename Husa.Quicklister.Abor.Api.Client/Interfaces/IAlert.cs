namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IAlert
    {
        Task<DataSet<AlertDetailResponse>> GetAsync(AlertType alertType, BaseAlertFilterRequest filters, CancellationToken token = default);

        Task<int> GetAlertTotal(IEnumerable<AlertType> alerts, CancellationToken token = default);
    }
}
