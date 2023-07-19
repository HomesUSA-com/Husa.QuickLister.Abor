namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class OpenHouseRequest
    {
        public OpenHouseType Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool Refreshments { get; set; }

        public bool Lunch { get; set; }
    }
}
