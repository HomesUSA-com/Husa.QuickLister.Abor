namespace Husa.Quicklister.Abor.Data.Queries
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationQueriesDbContext : ApplicationDbContext
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
