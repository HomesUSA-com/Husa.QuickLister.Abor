namespace Husa.Quicklister.Extensions.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public static class OpenHouseExtensions
    {
        public static void ImportOpenHouse<TFromOpenHouse, TToOpenHouse, TEntity>(this TEntity community, IEnumerable<TFromOpenHouse> openHouses)
            where TToOpenHouse : OpenHouse
            where TFromOpenHouse : OpenHouse
            where TEntity : IEntityOpenHouse<TToOpenHouse>
        {
            if (openHouses is null)
            {
                return;
            }

            community.OpenHouses.Clear();
            community.AddOpenHouses(openHouses);
        }

        public static void UpdateOpenHouse<TOpenHouse, TEntity>(this TEntity community, IEnumerable<TOpenHouse> openHouses)
            where TOpenHouse : OpenHouse
            where TEntity : IEntityOpenHouse<TOpenHouse>
        {
            if (openHouses is null)
            {
                return;
            }

            community.OpenHouses.Clear();
            community.AddOpenHouses(openHouses);
        }

        public static IEnumerable<SummaryField> SummaryOpenHouse(this ICollection<OpenHouseRecord> openHouses, IEnumerable<OpenHouseRecord> oldOpenHouses)
        {
            static IEnumerable<SummaryField> FieldSummary(IEnumerable<OpenHouseRecord> fieldElements, bool newValues)
            {
                foreach (var field in fieldElements)
                {
                    yield return new SummaryField(field.FieldType, newValues ? null : field, newValues ? field : null);
                }
            }

            if (oldOpenHouses == null || !oldOpenHouses.Any())
            {
                return FieldSummary(openHouses.OrderBy(oph => oph.Type).ThenBy(oph => oph.StartTime), true);
            }

            var summaryOpenHouses = new List<SummaryField>();

            if (openHouses.Count != oldOpenHouses.Count() || openHouses.Intersect(oldOpenHouses, new OpenHouseComparer()).Count() != openHouses.Count)
            {
                summaryOpenHouses.AddRange(FieldSummary(openHouses.OrderBy(oph => oph.Type).ThenBy(oph => oph.StartTime), true));
                summaryOpenHouses.AddRange(FieldSummary(oldOpenHouses.OrderBy(oph => oph.Type).ThenBy(oph => oph.StartTime), false));
            }

            return summaryOpenHouses;
        }
    }
}
