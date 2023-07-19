namespace Husa.Quicklister.Abor.Application.Interfaces.Office
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Office;

    public interface IOfficeService
    {
        Task ProcessDataFromDownloaderAsync(OfficeDto officeDto);
    }
}
