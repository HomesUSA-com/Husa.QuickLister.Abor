namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;

    public interface IResidentialIdxQueriesRepository
    {
        Task<IEnumerable<ResidentialIdxQueryResult>> FindByBuilderName(string builderName);
    }
}
