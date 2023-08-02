namespace Husa.Quicklister.Abor.Domain.Comparers
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingRoomComparer : IEqualityComparer<IProvideRoomInfo>
    {
        public bool Equals(IProvideRoomInfo x, IProvideRoomInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.RoomType == y.RoomType &&
                   x.Level == y.Level;
        }

        public int GetHashCode(IProvideRoomInfo room)
        {
            if (ReferenceEquals(room, null))
            {
                return 0;
            }

            int hashRoomType = room.RoomType.GetHashCode();
            int hashRoomLevel = room.Level.GetHashCode();

            return hashRoomType ^
                   hashRoomLevel;
        }
    }
}
