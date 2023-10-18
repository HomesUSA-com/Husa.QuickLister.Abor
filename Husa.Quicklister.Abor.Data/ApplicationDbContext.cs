namespace Husa.Quicklister.Abor.Data
{
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public const string Schema = "dbo";
        public const string ConnectionName = "HomesUSAConnection";

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SaleListing> ListingSale { get; set; }

        public virtual DbSet<SaleProperty> SaleProperty { get; set; }

        public virtual DbSet<Room> Room { get; set; }

        public virtual DbSet<OpenHouse> OpenHouse { get; set; }

        public virtual DbSet<CommunitySale> Community { get; set; }

        public virtual DbSet<Plan> Plan { get; set; }

        public virtual DbSet<Agent> Agent { get; set; }

        public virtual DbSet<Office> Office { get; set; }

        public virtual DbSet<ReverseProspect> ReverseProspect { get; set; }

        public virtual DbSet<CommunityEmployee> CommunityEmployee { get; set; }

        public virtual DbSet<ScrapedListing> ScrapedListing { get; set; }

        public virtual DbSet<ManagementTrace> ManagementTrace { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurations<ApplicationDbContext>();
        }
    }
}
