namespace Husa.Quicklister.Abor.Data.Queries
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Data.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationQueriesDbContext : ApplicationDbContext, IDbSetCommunityEmployee<CommunityEmployee>
    {
        public ApplicationQueriesDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override int SaveChanges()
        {
            // Throw if they try to call this
            throw new InvalidOperationException("This context is read-only.");
        }
    }
}
