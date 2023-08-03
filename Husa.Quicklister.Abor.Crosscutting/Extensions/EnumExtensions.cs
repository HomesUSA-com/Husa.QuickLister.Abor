namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Domain.Enums;
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

        public static SqFtSource? ToSqftSource(this string sqftSource)
        {
            if (string.IsNullOrWhiteSpace(sqftSource))
            {
                return null;
            }

            return sqftSource.ToEnumFromEnumMember<SqFtSource>();
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

        public static ICollection<ShowingInstructions> ToShowing(this string showing)
        {
            if (string.IsNullOrWhiteSpace(showing))
            {
                return new List<ShowingInstructions>();
            }

            return showing.CsvToEnum<ShowingInstructions>(true).ToList();
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

        public static CategoryType ToCategoryType(this PlanType type)
        {
            return type switch
            {
                PlanType.SingleFamily => CategoryType.SingleFamilyDetached,
                _ => CategoryType.Townhome,
            };
        }

        public static MarketStatuses ToStatus(this SpecStatus status)
        {
            return status switch
            {
                SpecStatus.Active => MarketStatuses.Active,
                SpecStatus.ActiveOption => MarketStatuses.ActiveOption,
                SpecStatus.Pending or SpecStatus.PendingContinuetoShow => MarketStatuses.Pending,
                _ => MarketStatuses.Sold,
            };
        }

        public static ICollection<GarageDescription> ToEntry(this GarageEntry entry, int? garageSpaces)
        {
            var description = GetGarageDescription(garageSpaces);
            var feature = entry switch
            {
                GarageEntry.Rear => GarageDescription.RearEntry,
                GarageEntry.Side => GarageDescription.SideEntry,
                _ => GarageDescription.NotApplicable,
            };

            if (feature != GarageDescription.NotApplicable)
            {
                description.Add(feature);
            }

            return description;
        }

        public static IList<GarageDescription> GetGarageDescription(int? garageSpaces)
        {
            if (!garageSpaces.HasValue || garageSpaces.Value <= 0)
            {
                return Array.Empty<GarageDescription>();
            }

            return garageSpaces.Value switch
            {
                1 => new[] { GarageDescription.OneCarGarage },
                2 => new[] { GarageDescription.TwoCarGarage },
                3 => new[] { GarageDescription.ThreeCarGarage },
                _ => new[] { GarageDescription.FourPlusCarGarage },
            };
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
