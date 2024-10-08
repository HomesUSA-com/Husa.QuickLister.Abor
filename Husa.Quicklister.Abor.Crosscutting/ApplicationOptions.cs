namespace Husa.Quicklister.Abor.Crosscutting
{
    using System.Globalization;
    using Husa.Extensions.Quickbooks.Models;

    public class ApplicationOptions : Husa.Quicklister.Extensions.Crosscutting.ApplicationOptions
    {
        private static readonly CultureInfo ApplicationCulture = new("en-US");

        public static CultureInfo ApplicationCultureInfo => ApplicationCulture;

        public FeatureFlags FeatureFlags { get; set; }

        public ListingRequestSettings ListingRequest { get; set; }

        public MediaAllowedSettings MediaAllowed { get; set; }

        public DownloaderAuthenticationOptions DownloaderConfiguration { get; set; }

        public InvoiceSettings InvoiceSettings { get; set; }
    }
}
