namespace Husa.Quicklister.Abor.Api.Contracts.Response.Xml
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using ResponseExtensions = Husa.Quicklister.Extensions.Api.Contracts.Response.Xml;

    public class XmlListingResponse : ResponseExtensions.XmlListingResponse<Cities, Counties>
    {
    }
}
