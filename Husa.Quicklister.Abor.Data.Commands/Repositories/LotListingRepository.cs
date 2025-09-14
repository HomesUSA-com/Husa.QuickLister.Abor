namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Specifications;
    using Husa.Extensions.Document.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class LotListingRepository : Repository<LotListing>, ILotListingRepository
    {
        public LotListingRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<ListingSaleRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<IEnumerable<LotListing>> GetListingsByCompanyId(Guid companyId)
        {
            var query = this.context.LotListing.FilterNotDeleted().FilterByCompany(companyId);
            return await query.ToListAsync();
        }

        public async Task<LotListing> GetListingByMlsNumber(string mlsNumber)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by mls number: {mlsNumber}", mlsNumber);
            return await this.context.LotListing
                 .FirstOrDefaultAsync(x => !x.IsDeleted && x.MlsNumber == mlsNumber);
        }

        public async Task<IEnumerable<LotListing>> GetListingsByAddress<TAddress>(TAddress address, IEnumerable<MarketStatuses> excludeStatuses = null)
            where TAddress : IProvideAddress
        {
            this.logger.LogInformation($"Starting to get the Lot listing with streetNumber: {address.StreetNumber} streetName: {address.StreetName} and zipcode: {address.ZipCode}");
            var query = this.context.LotListing
                .FilterNotDeleted()
                .Where(l => l.AddressInfo.StreetName == address.StreetName &&
                            l.AddressInfo.StreetNumber == address.StreetNumber &&
                            l.AddressInfo.ZipCode == address.ZipCode &&
                            l.AddressInfo.City == address.City);

            if (excludeStatuses != null)
            {
                query = query.Where(x => !excludeStatuses.Contains(x.MlsStatus));
            }

            return await query.ToListAsync();
        }

        public Task CustomSaveChangesAsync(Func<LotListing, LotListing, SavedChangesLog> logGenerator)
        => this.SaveChangesAsync();
    }
}
