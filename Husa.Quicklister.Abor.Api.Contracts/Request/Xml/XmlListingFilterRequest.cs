namespace Husa.Quicklister.Abor.Api.Contracts.Request.Xml
{
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class XmlListingFilterRequest : BaseAlertFilterRequest
    {
        public ImportStatus ImportStatus { get; set; }
    }
}
