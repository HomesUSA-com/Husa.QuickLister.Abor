namespace Husa.Quicklister.Abor.Domain.Comparers
{
    using System.Collections.Generic;
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

            return x.Type == y.Type &&
                   x.StartTime == y.StartTime &&
                   x.EndTime == y.EndTime &&
                   x.Refreshments == y.Refreshments;
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
            int hashRefreshments = openHouse.Refreshments.GetHashCode();

            return hashType ^
                   hashStartTime ^
                   hashEndTime ^
                   hashRefreshments;
        }
    }
}
