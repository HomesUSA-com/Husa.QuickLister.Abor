namespace Husa.Quicklister.Abor.Api.Contracts.Response.ReverseProspect
{
    using System;
    using System.Collections.Generic;

    public class ReverseProspectInformationResponse
    {
        public IEnumerable<ReverseProspectDataResponse> ReverseProspectData { get; set; }

        public DateTime? RequestedDate { get; set; }
    }
}
