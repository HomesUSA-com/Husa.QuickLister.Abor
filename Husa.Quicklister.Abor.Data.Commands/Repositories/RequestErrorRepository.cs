namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Extensions.Authorization;
    using Microsoft.Extensions.Logging;
    using QLExtension = Husa.Quicklister.Extensions.Data.Repositories;
    public class RequestErrorRepository : QLExtension.RequestErrorRepository<ApplicationDbContext>
    {
        public RequestErrorRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<RequestErrorRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
