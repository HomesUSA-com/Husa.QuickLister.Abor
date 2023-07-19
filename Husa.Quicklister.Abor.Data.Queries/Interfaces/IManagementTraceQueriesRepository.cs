namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models;

    public interface IManagementTraceQueriesRepository
    {
        Task<DataSet<ManagementTraceQueryResult>> GetAsync(Guid listingSaleId, string sortBy, int skip = 0, int take = 50);
    }
}
