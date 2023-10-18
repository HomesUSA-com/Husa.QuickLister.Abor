namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ReverseProspectRepository : Repository<ReverseProspect>, IReverseProspectRepository
    {
        public ReverseProspectRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<AgentRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public Task<ReverseProspect> GetReverseProspectByTrackingId(Guid listingId)
        {
            this.logger.LogInformation($"Starting to get Reverse prospect listing with id {listingId} from tracking table");

            return this.context.ReverseProspect
                .Where(x => x.ListingId == listingId)
                .Where(x => x.Status != ReverseProspectStatus.Requested)
                .Where(x => x.Status != ReverseProspectStatus.NotAvailable)
                .Where(x => EF.Functions.DateDiffDay(x.SysCreatedOn, DateTime.UtcNow) <= 3)
                .OrderByDescending(x => x.SysCreatedOn)
                .FirstOrDefaultAsync();
        }
    }
}
