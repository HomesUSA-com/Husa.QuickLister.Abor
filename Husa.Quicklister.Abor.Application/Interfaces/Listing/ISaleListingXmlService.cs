namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Contracts.Response;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

    public interface ISaleListingXmlService : ExtensionsInterfaces.ISaleListingXmlService<XmlListingDetailResponse>
    {
        Task<Guid> ProcessListingAsync(Guid xmlListingId, ImportActionType listAction);
        Task UpdateListingFromXmlAsync(Guid xmlListingId);
    }
}
