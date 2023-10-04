namespace Husa.Quicklister.Abor.Application.Interfaces.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;

    public interface IDownloaderService
    {
        Task ProcessDataFromDownloaderAsync(FullListingSaleDto listingSaleDto, IEnumerable<RoomDto> roomsDto, string agentMarketUniqueId);

        Task ProcessOpenHouseFromDownloaderAsync(string mlsNumber, IEnumerable<OpenHouseDto> openHousesDto);

        Task ImportMediaFromMlsAsync(Guid listingId);
    }
}
