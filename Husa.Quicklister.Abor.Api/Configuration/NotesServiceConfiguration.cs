namespace Husa.Quicklister.Abor.Api.Configuration
{
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Application.Services.Communities;
    using Husa.Quicklister.Extensions.Application.Services.LotListings;
    using Husa.Quicklister.Extensions.Application.Services.Notes;
    using Husa.Quicklister.Extensions.Application.Services.Plans;
    using Husa.Quicklister.Extensions.Application.Services.SaleListings;
    using Microsoft.Extensions.DependencyInjection;

    public static class NotesServiceConfiguration
    {
        public static IServiceCollection ConfigureNoteServices(this IServiceCollection services)
        {
            services.AddScoped<INotesBusService, NotesBusService<ApplicationOptions>>();
            services.AddScoped<ILotListingNotesService, LotListingNotesService<LotListing, ILotListingRepository>>();
            services.AddScoped<IPlanNotesService, PlanNotesService<Plan, IPlanRepository>>();
            services.AddScoped<ISaleListingNotesService, SaleListingNotesService<SaleListing, IListingSaleRepository>>();
            services.AddScoped<ICommunityNotesService, CommunityNotesService<CommunitySale, ICommunitySaleRepository>>();
            return services;
        }
    }
}
