namespace Husa.Quicklister.Abor.Crosscutting
{
    public class FeatureFlags
    {
        public bool IsDownloaderEnabled { get; set; }

        public bool IsXmlBusHandlerEnabled { get; set; }

        public bool FindEmailLeads { get; set; }

        public bool EnableBusHandlers { get; set; }

        public bool AllowManualListingUnlock { get; set; }
    }
}
