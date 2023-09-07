namespace Husa.Quicklister.Abor.Application.Interfaces.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Application.Models;

    public interface IDownloaderService
    {
        Task ProcessDataFromDownloaderAsync(FullListingSaleDto listingSaleDto, IEnumerable<RoomDto> roomsDto, string sellingAgent);

        Task ProcessOpenHouseFromDownloaderAsync(string mlsNumber, IEnumerable<OpenHouseDto> openHousesDto);

        Task ImportMediaFromMlsAsync(Guid listingId);
    }
}
