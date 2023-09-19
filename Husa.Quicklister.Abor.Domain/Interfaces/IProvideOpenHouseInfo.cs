namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface IProvideOpenHouseInfo
    {
        OpenHouseType Type { get; set; }

        TimeSpan StartTime { get; set; }

        TimeSpan EndTime { get; set; }

        ICollection<Refreshments> Refreshments { get; set; }
    }
}
