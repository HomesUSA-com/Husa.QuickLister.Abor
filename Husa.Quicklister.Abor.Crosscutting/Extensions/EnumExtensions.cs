namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Domain.Enums.Xml;
    using States = Husa.Extensions.Common.Enums.States;

    public static class EnumExtensions
    {
        public static States? ToState(this string state, bool isExactValue = true)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                return null;
            }

            return isExactValue ?
                state.ToEnumFromEnumMember<States>() :
                state.GetEnumFromText<States>();
        }

        public static Cities? ToCity(this string city, bool isExactValue = true)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            return isExactValue ?
                city.ToEnumFromEnumMember<Cities>() :
                city.GetEnumFromText<Cities>();
        }

        public static Counties? ToCounty(this string county, bool isExactValue = true)
        {
            if (string.IsNullOrWhiteSpace(county))
            {
                return null;
            }

            return isExactValue ?
                county.ToEnumFromEnumMember<Counties>() :
                county.GetEnumFromText<Counties>();
        }

        public static ListType? ToListType(this string sqftSource)
        {
            if (string.IsNullOrWhiteSpace(sqftSource))
            {
                return null;
            }

            return sqftSource.ToEnumFromEnumMember<ListType>();
        }

        public static MarketStatuses? ToMarketStatus(this string marketStatus)
        {
            if (string.IsNullOrWhiteSpace(marketStatus))
            {
                return null;
            }

            return marketStatus.ToEnumFromEnumMember<MarketStatuses>();
        }

        public static Stories? ToStories(this string stories)
        {
            if (string.IsNullOrWhiteSpace(stories))
            {
                return null;
            }

            return stories.ToEnumFromEnumMember<Stories>();
        }

        public static Stories? ToStories(this decimal? stories)
        {
            if (!stories.HasValue || stories.Value <= 0)
            {
                return null;
            }

            if (stories.Value == decimal.Parse("1.5"))
            {
                return Stories.OnePointFive;
            }

            return decimal.ToInt32(stories.Value) switch
            {
                1 => Stories.One,
                2 => Stories.Two,
                _ => Stories.ThreePlus,
            };
        }

        public static MarketStatuses ToStatus(this SpecStatus status)
        {
            return status switch
            {
                SpecStatus.Active => MarketStatuses.Active,
                SpecStatus.ActiveOption => MarketStatuses.ActiveUnderContract,
                SpecStatus.Pending or SpecStatus.PendingContinuetoShow => MarketStatuses.Pending,
                _ => MarketStatuses.Closed,
            };
        }

        public static ICollection<GarageDescription> ToEntry(this GarageEntry entry)
        {
            var description = new List<GarageDescription>();
            GarageDescription? feature = entry switch
            {
                GarageEntry.Rear => GarageDescription.GarageFacesRear,
                GarageEntry.Side => GarageDescription.GarageFacesSide,
                _ => null,
            };

            if (feature.HasValue)
            {
                description.Add(feature.Value);
            }

            return description;
        }

        public static IEnumerable<string> ToStringCollectionFromEnumMembers<T>(this IEnumerable<T> enumElements)
            where T : Enum
        {
            if (enumElements == null || !enumElements.Any())
            {
                return Array.Empty<string>();
            }

            return enumElements.Select((T enumElement) => enumElement.ToStringFromEnumMember());
        }
    }
}
