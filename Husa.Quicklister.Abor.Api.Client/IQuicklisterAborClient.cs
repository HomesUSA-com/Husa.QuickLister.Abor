namespace Husa.Quicklister.Abor.Api.Client
{
    using Husa.Quicklister.Abor.Api.Client.Interfaces;

    public interface IQuicklisterAborClient
    {
        public IListingSaleRequest ListingSaleRequest { get; }
        public ISaleListing SaleListing { get; }
        public ISaleCommunity SaleCommunity { get; }
        public IPlan Plan { get; }
        public IAlert Alert { get; }
        public IReport Report { get; }
    }
}
