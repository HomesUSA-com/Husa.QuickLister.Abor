namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Domain.Enums;
    using AlertExtension = Husa.Quicklister.Extensions.Data.Queries.Interfaces;

    public interface IAlertQueriesRepository : AlertExtension.IAlertQueriesRepository<DetailAlertQueryResult, MarketStatuses>
    {
    }
}
