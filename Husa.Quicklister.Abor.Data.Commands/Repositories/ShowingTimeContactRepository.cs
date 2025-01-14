namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ShowingTimeContactRepository : Repository<ShowingTimeContact, ApplicationDbContext>, IShowingTimeContactRepository
    {
        public ShowingTimeContactRepository(
            ApplicationDbContext context,
            IUserContextProvider userContextProvider,
            ILogger<ShowingTimeContactRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task AssignToCommunity(Guid contactId, Guid communityId, int? order = null)
        {
            var contact = this.context.CommunityShowingTimeContacts
                .FirstOrDefault(x => x.ContactId == contactId && x.ScopeId == communityId);

            if (contact == null)
            {
                await this.context.CommunityShowingTimeContacts.AddAsync(new()
                {
                    ScopeId = communityId,
                    ContactId = contactId,
                    Order = order ?? 1,
                });
            }
            else if (order is not null)
            {
                contact.Order = order.Value;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task AssignToListing(Guid contactId, Guid listingId, int? order = null)
        {
            var contact = this.context.CommunityShowingTimeContacts
                .FirstOrDefault(x => x.ContactId == contactId && x.ScopeId == listingId);

            if (contact == null)
            {
                await this.context.ListingShowingTimeContacts.AddAsync(new()
                {
                    ScopeId = listingId,
                    ContactId = contactId,
                    Order = order ?? 1,
                });
            }
            else if (order is not null)
            {
                contact.Order = order.Value;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task RemoveFromCommunity(Guid contactId, Guid? communityId = null)
        {
            var query = this.context.CommunityShowingTimeContacts.Where(c => c.ContactId == contactId);

            if (communityId is not null)
            {
                query = query.Where(c => c.ScopeId == communityId.Value);
            }

            var entities = await query.ToArrayAsync();

            this.context.CommunityShowingTimeContacts.RemoveRange(entities);
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveFromListing(Guid contactId, Guid? listingId = null)
        {
            var query = this.context.ListingShowingTimeContacts.Where(c => c.ContactId == contactId);

            if (listingId is not null)
            {
                query = query.Where(c => c.ScopeId == listingId.Value);
            }

            var entities = await query.ToArrayAsync();

            this.context.ListingShowingTimeContacts.RemoveRange(entities);
            await this.context.SaveChangesAsync();
        }
    }
}
