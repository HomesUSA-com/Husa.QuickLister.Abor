namespace Husa.Quicklister.Abor.Api.Client
{
    using System;
    using System.Net.Http;
    using Husa.Extensions.Api.Client;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Client.Interfaces.LotListing;
    using Husa.Quicklister.Abor.Api.Client.Resources;
    using Husa.Quicklister.Abor.Api.Client.Resources.LotListing;
    using Microsoft.Extensions.Logging;
    using ExtensionsClient = Husa.Quicklister.Extensions.Api.Client;

    public class QuicklisterAborClient : HusaStandardClient, IQuicklisterAborClient
    {
        public QuicklisterAborClient(ILoggerFactory loggerFactory, HttpClient httpClient)
            : base(httpClient)
        {
            var logger = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.ListingSaleRequest = new ListingSaleRequest(this, logger.CreateLogger<ListingSaleRequest>());
            this.ListingLotRequest = new ListingLotRequest(this, logger.CreateLogger<ListingLotRequest>());
            this.SaleListing = new SaleListing(this, logger.CreateLogger<SaleListing>());
            this.SaleCommunity = new SaleCommunity(this, logger.CreateLogger<SaleCommunity>());
            this.Plan = new Plan(this, logger.CreateLogger<Plan>());
            this.Alert = new ExtensionsClient.Resources.Alert(this, logger.CreateLogger<ExtensionsClient.Resources.Alert>());
            this.Report = new Report(this, logger.CreateLogger<Report>());
            this.Xml = new Xml(this, logger.CreateLogger<Xml>());
        }

        public IListingSaleRequest ListingSaleRequest { get; }
        public IListingLotRequest ListingLotRequest { get; }
        public ISaleListing SaleListing { get; }
        public ISaleCommunity SaleCommunity { get; }
        public IPlan Plan { get; }
        public ExtensionsClient.Interfaces.IAlert Alert { get; }
        public IReport Report { get; }
        public IXml Xml { get; }
    }
}
