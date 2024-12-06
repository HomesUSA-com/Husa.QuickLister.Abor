namespace Husa.Quicklister.Dfw.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Extensions.Domain.Enums.Extensions;

    public static class JsonImportOpenHouseExtensions
    {
        public static ICollection<OpenHouse> ToOpenHouses(this ICollection<OpenHouseResponse> jsonOpenHouse)
            => jsonOpenHouse.Select(x => x.ToOpenHouse()).ToList();

        private static OpenHouse ToOpenHouse(this OpenHouseResponse jsonOpenHouse)
        {
            return new OpenHouse(jsonOpenHouse.Day.ToOpenHouseType(), startTime: jsonOpenHouse.StartTime, endTime: jsonOpenHouse.EndTime);
        }
    }
}
