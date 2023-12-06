namespace Husa.Quicklister.Abor.Api.Contracts.Request.Plan
{
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class PlanRequestFilter : BaseFilterRequest
    {
        public string SearchBy { get; set; }
        public XmlStatus? XmlStatus { get; set; }
    }
}
