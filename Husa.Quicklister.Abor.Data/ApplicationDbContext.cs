namespace Husa.Quicklister.Abor.Data
{
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Data;
    using Husa.Quicklister.Extensions.Data.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Entities.Agent;
    using Microsoft.EntityFrameworkCore;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;

    public class ApplicationDbContext :
        QlApplicationDbContext<CommunityEmployee>,
        IDbSetSaleListing<SaleListing>,
        IDbSetXmlSaleListing<SaleListing>,
        IDbSetAgent,
        IDbSetOffice<Office, OfficeValueObject, Cities, StateOrProvince>
    {
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

        public virtual DbSet<ScrapedListing> ScrapedListing { get; set; }

        public virtual DbSet<ManagementTrace> ManagementTrace { get; set; }

        public virtual DbSet<LotListing> LotListing { get; set; }

        public virtual DbSet<LotManagementTrace> LotManagementTrace { get; set; }

        public virtual DbSet<ShowingTimeContact> ShowingTimeContacts { get; set; }

        public virtual DbSet<CommunityShowingTimeContact> CommunityShowingTimeContacts { get; set; }

        public virtual DbSet<ListingShowingTimeContact> ListingShowingTimeContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurations<ApplicationDbContext>();
        }
    }
}
