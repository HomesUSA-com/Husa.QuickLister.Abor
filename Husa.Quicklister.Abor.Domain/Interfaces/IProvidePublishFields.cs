namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface IProvidePublishFields
    {
        ActionType? PublishType { get; set; }

        Guid? PublishUser { get; set; }

        MarketStatuses? PublishStatus { get; set; }

        DateTime? PublishDate { get; set; }
    }
}
