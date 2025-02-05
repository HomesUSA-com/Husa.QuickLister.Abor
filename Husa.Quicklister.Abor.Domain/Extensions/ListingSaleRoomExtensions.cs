namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public static class ListingSaleRoomExtensions
    {
        public static IEnumerable<PlanRoom> ConvertToPlanRoom(this ICollection<ListingSaleRoom> listingSaleRooms)
            => listingSaleRooms.Select(room => new PlanRoom(
                    room.Id, room.RoomType, room.Level, room.Features, room.Description));
    }
}
