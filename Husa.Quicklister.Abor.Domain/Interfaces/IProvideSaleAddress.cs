namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    public interface IProvideSaleAddress : IProvideAddress
    {
        string UnitNumber { get; set; }
    }
}
