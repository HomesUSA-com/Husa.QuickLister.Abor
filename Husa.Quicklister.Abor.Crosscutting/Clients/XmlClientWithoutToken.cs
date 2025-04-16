namespace Husa.Quicklister.Abor.Crosscutting.Clients
{
    using System.Net.Http;
    using Husa.Xml.Api.Client;
    using Microsoft.Extensions.Logging;

    public class XmlClientWithoutToken : XmlClient, IXmlClientWithoutToken
    {
        public XmlClientWithoutToken(ILoggerFactory loggerFactory, HttpClient httpClient)
            : base(loggerFactory, httpClient)
        {
        }
    }
}
