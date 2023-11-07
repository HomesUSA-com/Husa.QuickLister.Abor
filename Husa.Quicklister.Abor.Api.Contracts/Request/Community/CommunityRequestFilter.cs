namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class CommunityRequestFilter : BaseFilterRequest
    {
        public string SearchBy { get; set; }
        public string Name { get; set; }
        public XmlStatus? XmlStatus { get; set; }
    }
}
