namespace Husa.Quicklister.Abor.Domain.Comparers
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
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

            var comparerFeatures = (x.Features ?? new List<RoomFeatures>()).Except(y.Features ?? new List<RoomFeatures>());

            return x.RoomType == y.RoomType &&
                   x.Level == y.Level &&
                   x.Length == y.Length &&
                   x.Width == y.Width &&
                   !comparerFeatures.Any();
        }

        public int GetHashCode(IProvideRoomInfo room)
        {
            if (ReferenceEquals(room, null))
            {
                return 0;
            }

            int hashRoomType = room.RoomType.GetHashCode();
            int hashRoomLevel = room.Level.GetHashCode();
            int hashRoomLength = room.Length.GetHashCode();
            int hashWidthLevel = room.Width.GetHashCode();
            int hashFeatures = room.Features.GetHashCode();

            return hashRoomType ^
                   hashRoomLevel ^
                   hashRoomLength ^
                   hashWidthLevel ^
                   hashFeatures;
        }
    }
}
