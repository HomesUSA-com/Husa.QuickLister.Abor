namespace Husa.Quicklister.Abor.Api.Contracts.Request.Xml
{
    using Husa.Quicklister.Abor.Domain.Enums.Xml;

    public class XmlListingFilterRequest : BaseAlertFilterRequest
    {
        public ImportStatus ImportStatus { get; set; }
    }
}
