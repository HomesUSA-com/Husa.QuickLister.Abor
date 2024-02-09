namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class ManagementTraceQueriesRepository : IManagementTraceQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserRepository userQueriesRepository;

        public ManagementTraceQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserRepository userQueriesRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<DataSet<ManagementTraceQueryResult>> GetAsync(Guid listingSaleId, string sortBy, int skip = 0, int take = 50)
        {
            var query = this.context.ManagementTrace
                .Where(trace => trace.SaleListingId == listingSaleId)
                .FilterNotDeleted();

            var total = await query.CountAsync();

            var data = await query
                .Select(trace => new ManagementTraceQueryResult
                {
                    Id = trace.Id,
                    SysCreatedOn = trace.SysCreatedOn,
                    SysCreatedBy = trace.SysCreatedBy,
                    SysModifiedBy = trace.SysModifiedBy,
                    ListingSaleId = trace.SaleListingId,
                    IsManuallyManaged = trace.IsManuallyManaged,
                })
                .ApplySortByFields(sortBy)
                .ApplyPaginationFilter(skip, take)
                .ToListAsync();

            await this.userQueriesRepository.FillUsersNameAsync(data);
            return new DataSet<ManagementTraceQueryResult>(data, total);
        }
    }
}
