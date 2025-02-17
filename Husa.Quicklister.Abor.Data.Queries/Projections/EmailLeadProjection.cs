namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Projections;

    public static class EmailLeadProjection
    {
        public static Husa.Quicklister.Extensions.Data.Queries.Models.EmailLeadQueryResult ToProjectionEmailLead<T>(this T community)
           where T : CommunitySale
           => community.EmailLead.ToProjectionEmailLead();
    }
}
