namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Microsoft.EntityFrameworkCore;

    public static class CommunitySpecifications
    {
        public static IQueryable<CommunitySale> ApplySearchByFilter(this IQueryable<CommunitySale> query, string searchByFilter)
        {
            if (string.IsNullOrEmpty(searchByFilter))
            {
                return query;
            }

            return query.Where(x => x.ProfileInfo.Name.Contains(searchByFilter)
                || x.Property.ZipCode.StartsWith(searchByFilter)
                || EF.Functions.Like((string)(object)x.Property.City, $"%{searchByFilter}%"));
        }

        public static IQueryable<CommunitySale> FilterByCommunityName(this IQueryable<CommunitySale> query, string communityName)
        {
            return string.IsNullOrEmpty(communityName) ? query : query.Where(p => p.ProfileInfo.Name.Contains(communityName));
        }
    }
}
