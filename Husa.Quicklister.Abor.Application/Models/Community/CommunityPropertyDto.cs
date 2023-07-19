namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityPropertyDto
    {
        public Cities? City { get; set; }

        public Counties? County { get; set; }

        public MlsArea? MlsArea { get; set; }

        public string MapscoGrid { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }
    }
}
