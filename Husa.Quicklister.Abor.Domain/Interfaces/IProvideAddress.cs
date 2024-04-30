namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideAddress
    {
        string StreetNumber { get; set; }
        string StreetName { get; set; }
        Cities City { get; set; }
        string ZipCode { get; set; }
    }
}
