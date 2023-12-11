namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;

    public interface IImportMlsMediaMessagingService
    {
        Task SendMessage<T>(IEnumerable<T> messages, string userId, MarketCode market, string correlationId = null, bool dispose = true)
            where T : IProvideBusEvent;
    }
}
