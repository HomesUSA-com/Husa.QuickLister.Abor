namespace Husa.Quicklister.Abor.Application.Interfaces.OpenHouse
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models;

    public interface IOpenHouseService
    {
        Task ProcessData(string mlsNumber, OpenHouseDto openHouseDto);
    }
}
