namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Domain.Entities;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected readonly ApplicationDbContext context;
        protected readonly ILogger logger;

        private readonly IEnumerable<EntityState> excludedStates = new[] { EntityState.Detached, EntityState.Unchanged };
        private readonly IUserContextProvider userContextProvider;

        public Repository(
            ApplicationDbContext context,
            IUserContextProvider userContextProvider,
            ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
        }

        public Task<TEntity> GetById(Guid id, bool filterByCompany = false)
        {
            if (id == Guid.Empty)
            {
                this.logger.LogInformation("{parameterId} must not be null {entityType}", nameof(id), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(id), $"{nameof(id)} must not be null {typeof(TEntity).Name}");
            }

            return this.FindEntityById(id, filterByCompany);
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                this.logger.LogError("{parameterName} entity must not be nulll {entityType}", nameof(entity), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} entity must not be nulll {typeof(TEntity).Name}");
            }

            return this.AddToContextAsync(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                this.logger.LogError("{parameterName} entity must not be nulll {entityType}", nameof(entity), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} entity must not be nulll {typeof(TEntity).Name}");
            }

            this.context.Update(entity);
            return this.SaveChangesAsync(entity);
        }

        public Task SaveChangesAsync()
        {
            if (!this.context.ChangeTracker.HasChanges())
            {
                return Task.CompletedTask;
            }

            return this.ApplyChangesAsync();
        }

        public Task SaveChangesAsync(TEntity entity)
        {
            if (entity == null)
            {
                this.logger.LogError("{parameterName} entity must not be nulll {entityType}", nameof(entity), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} entity must not be nulll {typeof(TEntity).Name}");
            }

            if (!this.context.ChangeTracker.HasChanges())
            {
                return Task.CompletedTask;
            }

            return this.ApplyChangesAsync();
        }

        public void Attach(TEntity entity)
        {
            if (entity == null)
            {
                this.logger.LogError("{parameterName} entity must not be null {entityType}", nameof(entity), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} entity must not be null {typeof(TEntity).Name}");
            }

            this.context.Attach(entity);
        }

        public void Attach(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                this.logger.LogError("{parameterName} entity must not be null {entityType}", nameof(entities), typeof(TEntity).Name);
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} entity must not be null {typeof(TEntity).Name}");
            }

            if (!entities.Any())
            {
                this.logger.LogInformation("There were no elements of type {entityType} to attach", typeof(TEntity).Name);
                return;
            }

            this.context.AttachRange(entities);
        }

        private async Task<TEntity> AddToContextAsync(TEntity entity)
        {
            await this.context.AddAsync(entity);
            await this.SaveChangesAsync(entity);
            return entity;
        }

        private async Task ApplyChangesAsync()
        {
            var entities = this.context.ChangeTracker
                .Entries<Entity>()
                .Where(entry => !this.excludedStates.Contains(entry.State));

            foreach (var modifiedEntity in entities)
            {
                var trackedEntity = modifiedEntity.Entity;
                var userId = this.userContextProvider.GetCurrentUserId();
                if (modifiedEntity.State == EntityState.Added)
                {
                    trackedEntity.SysCreatedBy = userId;
                }

                trackedEntity.UpdateTrackValues(userId);
            }

            await this.context.SaveChangesAsync();
        }

        private async Task<TEntity> FindEntityById(Guid id, bool filterByCompany)
        {
            var currentUser = this.userContextProvider.GetCurrentUser();

            return await this.context
                .Set<TEntity>()
                .FilterById(id)
                .FilterByCompany(currentUser.CompanyId, filterByCompany && (currentUser.UserRole == UserRole.User || currentUser.CompanyId.HasValue))
                .SingleOrDefaultAsync();
        }
    }
}
