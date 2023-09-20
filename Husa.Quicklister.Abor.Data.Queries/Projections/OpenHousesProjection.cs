namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;

    public static class OpenHousesProjection
    {
        public static ICollection<OpenHousesQueryResult> ToProjectionOpenHouses<T>(this ICollection<T> openHouses)
            where T : OpenHouse
        {
            var openHouseCollection = new List<OpenHousesQueryResult>();
            foreach (var openH in openHouses)
            {
                var openHouse = new OpenHousesQueryResult
                {
                    Type = openH.Type,
                    EndTime = openH.EndTime,
                    StartTime = openH.StartTime,
                    Refreshments = openH.Refreshments,
                };

                openHouseCollection.Add(openHouse);
            }

            return openHouseCollection;
        }
    }
}
