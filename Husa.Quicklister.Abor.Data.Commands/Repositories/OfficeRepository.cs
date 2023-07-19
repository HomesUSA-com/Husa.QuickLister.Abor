namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        public OfficeRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<OfficeRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<Office> GetByMarketUniqueId(string marketUniqueId)
        {
            this.logger.LogInformation($"Starting to get office with uid {marketUniqueId}");
            return await this.context.Office.Where(x => x.OfficeValue.MarketUniqueId == marketUniqueId).SingleOrDefaultAsync();
        }
    }
}
