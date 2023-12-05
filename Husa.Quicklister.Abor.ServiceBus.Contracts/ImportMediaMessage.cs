namespace Husa.Quicklister.Abor.ServiceBus.Contracts
{
    using System;
    using Husa.Extensions.ServiceBus.Interfaces;

    public class ImportMediaMessage : IProvideBusEvent
    {
        public ImportMediaMessage()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string MlsId { get; set; }
        public Guid EntityId { get; set; }
        public int MediaLimit { get; set; }
    }
}
