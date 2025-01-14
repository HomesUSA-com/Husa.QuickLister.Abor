namespace Husa.Quicklister.Abor.Domain.Interfaces.SaleListing
{
    public interface IProvideSaleShowing : IProvideShowingInfo
    {
        public bool EnableOpenHouses { get; set; }
        public bool ShowOpenHousesPending { get; set; }
    }
}
