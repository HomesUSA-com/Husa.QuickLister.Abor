namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;

    public interface IKpiQueriesRequestProvider
        : IKpiQueryRequestsProvider<
            SaleListing,
            SaleProperty,
            MarketStatuses>
    {
    }
}
