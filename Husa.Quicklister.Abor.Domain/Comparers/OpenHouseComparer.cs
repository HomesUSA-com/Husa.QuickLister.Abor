namespace Husa.Quicklister.Abor.Domain.Comparers
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class OpenHouseComparer : IEqualityComparer<IProvideOpenHouseInfo>
    {
        public bool Equals(IProvideOpenHouseInfo x, IProvideOpenHouseInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            var arePropertiesEqual = x.Type == y.Type &&
                       x.StartTime == y.StartTime &&
                       x.EndTime == y.EndTime;

            var refreshments = x.Refreshments ?? new List<Refreshments>();
            var compareRefreshments = new HashSet<Refreshments>(refreshments);
            compareRefreshments.SymmetricExceptWith(refreshments);
            var areRefreshmentsEqual = compareRefreshments.Count == 0;

            return arePropertiesEqual && areRefreshmentsEqual;
        }

        public int GetHashCode(IProvideOpenHouseInfo openHouse)
        {
            if (ReferenceEquals(openHouse, null))
            {
                return 0;
            }

            int hashType = openHouse.Type.GetHashCode();
            int hashStartTime = openHouse.StartTime.GetHashCode();
            int hashEndTime = openHouse.EndTime.GetHashCode();

            if (openHouse.Refreshments != null)
            {
                int hashRefreshments = 0;
                foreach (var refreshment in openHouse.Refreshments)
                {
                    hashRefreshments ^= refreshment.GetHashCode();
                }

                return hashType ^ hashStartTime ^ hashEndTime ^ hashRefreshments;
            }
            else
            {
                return hashType ^ hashStartTime ^ hashEndTime;
            }
        }
    }
}
