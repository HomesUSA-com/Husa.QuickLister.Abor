namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PropertyQueryResult
    {
        public Cities? City { get; set; }

        public Counties? County { get; set; }

        public MlsArea? MlsArea { get; set; }

        public string MapscoGrid { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }
    }
}
