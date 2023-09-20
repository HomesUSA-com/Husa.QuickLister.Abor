namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class OpenHouseResponse
    {
        public OpenHouseType Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public ICollection<Refreshments> Refreshments { get; set; }
    }
}
