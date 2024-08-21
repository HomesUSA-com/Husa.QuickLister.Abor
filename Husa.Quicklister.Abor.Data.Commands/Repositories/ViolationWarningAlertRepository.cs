namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Extensions.Authorization;
    using Microsoft.Extensions.Logging;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Repositories;
    public class ViolationWarningAlertRepository : ExtensionRepositories.ViolationWarningAlertRepository<ApplicationDbContext>
    {
        public ViolationWarningAlertRepository(
            ApplicationDbContext context,
            IUserContextProvider userContextProvider,
            ILogger<ViolationWarningAlertRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
