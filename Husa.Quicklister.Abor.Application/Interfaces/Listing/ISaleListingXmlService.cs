namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public interface ISaleListingXmlService
    {
        Task<Guid> ProcessListingAsync(Guid xmlListingId, ListActionType listAction);
        Task UpdateListingFromXmlAsync(Guid xmlListingId);
        Task DeleteListingAsync(Guid xmlListingId);
        Task ListLaterAsync(Guid xmlListingId, DateTime listOn);
        Task RestoreListingAsync(Guid xmlListingId);
        Task AutoMatchListingFromXmlAsync(Guid xmlListingId);
    }
}
