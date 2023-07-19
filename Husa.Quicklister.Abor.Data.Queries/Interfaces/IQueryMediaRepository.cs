namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public interface IQueryMediaRepository
    {
        Task<IEnumerable<SummarySection>> GetMediaSummary(Guid currentRequestId, Guid? lastestCompletedRequestId);
    }
}
