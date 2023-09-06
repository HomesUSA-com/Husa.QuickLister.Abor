namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    public interface IProvideGeocodes
    {
        decimal? Latitude { get; set; }

        decimal? Longitude { get; set; }
    }
}
