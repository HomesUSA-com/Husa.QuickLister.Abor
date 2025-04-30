namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Domain.Entities;
    using Microsoft.Extensions.Logging;
    using ExtensionsRepo = Husa.Quicklister.Extensions.Data.Repositories;

    public class Repository<TEntity> : ExtensionsRepo.Repository<TEntity, ApplicationDbContext>
        where TEntity : Entity, IProvideCompany
    {
        public Repository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
