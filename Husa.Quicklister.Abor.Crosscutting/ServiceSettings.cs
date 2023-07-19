namespace Husa.Quicklister.Abor.Crosscutting
{
    using System.ComponentModel.DataAnnotations;

    public class ServiceSettings
    {
        [Required]
        public string CompanyServicesManager { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string Media { get; set; }

        [Required]
        public string PhotoService { get; set; }

        [Required]
        public string ReverseProspect { get; set; }

        [Required]
        public string Downloader { get; set; }

        [Required]
        public string MigrationService { get; set; }

        [Required]
        public string XmlService { get; set; }
    }
}
