namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IAlertQueriesRepository
    {
        Task<DataSet<DetailAlertQueryResult>> GetAsync(AlertType alertType, BaseAlertQueryFilter filter);
        Task<int> GetTotal(IEnumerable<AlertType> alerts);
    }
}
