namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Contracts.Response;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

    public interface ISaleListingXmlService : ExtensionsInterfaces.ISaleListingXmlService<XmlListingDetailResponse>
    {
        Task<Guid> ProcessListingAsync(Guid xmlListingId, ListActionType listAction);
        Task UpdateListingFromXmlAsync(Guid xmlListingId);
    }
}
