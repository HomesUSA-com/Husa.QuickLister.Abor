namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtension = Husa.Quicklister.Extensions.Data.Repositories;

    public class OfficeRepository : QLExtension.OfficeRepository<Office, OfficeValueObject, Cities, StateOrProvince, ApplicationDbContext>, IOfficeRepository
    {
        public OfficeRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<OfficeRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
