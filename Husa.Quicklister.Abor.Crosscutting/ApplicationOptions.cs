namespace Husa.Quicklister.Abor.Crosscutting
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Husa.Extensions.Quickbooks.Models;

    public class ApplicationOptions
    {
        public const string Section = "Application";

        private static readonly CultureInfo ApplicationCulture = new("en-US");

        public static CultureInfo ApplicationCultureInfo => ApplicationCulture;

        [Required(AllowEmptyStrings = false)]
        public string QuicklisterUIUri { get; set; }

        public ServiceSettings Services { get; set; }

        public FeatureFlags FeatureFlags { get; set; }

        public ListingRequestSettings ListingRequest { get; set; }

        public MediaAllowedSettings MediaAllowed { get; set; }

        public DownloaderAuthenticationOptions DownloaderConfiguration { get; set; }

        public InvoiceSettings InvoiceSettings { get; set; }
    }
}
