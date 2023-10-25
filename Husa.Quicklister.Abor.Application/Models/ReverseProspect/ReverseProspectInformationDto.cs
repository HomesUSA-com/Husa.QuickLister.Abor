namespace Husa.Quicklister.Abor.Application.Models.ReverseProspect
{
    using System;
    using System.Collections.Generic;

    public class ReverseProspectInformationDto
    {
        public IEnumerable<ReverseProspectDataDto> ReverseProspectData { get; set; }

        public DateTime? RequestedDate { get; set; }
    }
}
