namespace Husa.Quicklister.Abor.Application.Interfaces.Downloader
{
    using System.Threading.Tasks;

    public interface IListingService
    {
        Task ProcessData(string entityKey, bool processFullListing);
    }
}
