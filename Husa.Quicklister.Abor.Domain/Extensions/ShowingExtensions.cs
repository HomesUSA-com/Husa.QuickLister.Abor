namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;

    public static class ShowingExtensions
    {
        public static void EnableOpenHouse(this IProvideSaleShowing info, bool showOpenHouseWhenPending = false)
        {
            info.EnableOpenHouses = true;

            if (showOpenHouseWhenPending)
            {
                info.ShowOpenHousesPending = true;
            }
        }
    }
}
