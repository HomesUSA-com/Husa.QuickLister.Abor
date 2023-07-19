namespace Husa.Quicklister.Abor.Application.Interfaces.Media
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.ServiceBus.Interfaces;

    public interface IMediaMessagingService
    {
        Task SendMessage<T>(IEnumerable<T> messages, string userId, string correlationId = null, bool dispose = true)
            where T : IProvideBusEvent;
    }
}
