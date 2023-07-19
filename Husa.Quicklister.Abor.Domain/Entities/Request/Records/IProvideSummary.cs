namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public interface IProvideSummary
    {
        SummarySection GetSummary<T>(T entity)
            where T : class;
    }
}
