namespace Husa.Quicklister.Abor.Domain.Interfaces.SaleListing
{
    public interface IProvideSaleFeature : IProvideFeature
    {
        string PropertyDescription { get; set; }
        bool IsAIGeneratedPropertyDescription { get; set; }
    }
}
