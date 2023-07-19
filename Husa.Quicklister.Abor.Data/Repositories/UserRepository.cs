namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Request;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.Cache;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Response = Husa.CompanyServicesManager.Api.Contracts.Response;

    public class UserRepository : IUserRepository
    {
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly ICache cache;

        public UserRepository(ICache memoryCache, IServiceSubscriptionClient serviceSubscriptionClient)
        {
            this.cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
        }

        public Task<UserContext> GetUserContextAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<RoleEmployee> GetUserRoleEmployee(Guid userId, Guid companyId)
        {
            throw new NotImplementedException();
        }

        public async Task FillUsersNameAsync(IEnumerable<IProvideQuicklisterUserInfo> userInfos)
        {
            if (!userInfos.Any())
            {
                return;
            }

            var userIds = userInfos.SelectMany((user) =>
            {
                var ids = new List<Guid>();
                if (user.SysCreatedBy.HasValue && !user.SysCreatedBy.Value.Equals(Guid.Empty))
                {
                    ids.Add(user.SysCreatedBy.Value);
                }

                if (user.SysModifiedBy.HasValue && !user.SysModifiedBy.Value.Equals(Guid.Empty))
                {
                    ids.Add(user.SysModifiedBy.Value);
                }

                if (user.LockedBy.HasValue && !user.LockedBy.Value.Equals(Guid.Empty))
                {
                    ids.Add(user.LockedBy.Value);
                }

                return ids;
            }).Distinct().ToList();

            var users = await this.GetUsersById(userIds);
            if (!users.Any())
            {
                return;
            }

            var information = from entity in userInfos
                               join createdByUser in users on entity.SysCreatedBy equals createdByUser.Id into createdMatch
                               from createdByUser in createdMatch.DefaultIfEmpty()
                               join modifiedByUser in users on entity.SysModifiedBy equals modifiedByUser.Id into modifiedMatch
                               from modifiedByUser in modifiedMatch.DefaultIfEmpty()
                              join lockedByUser in users on entity.LockedBy equals lockedByUser.Id into lockedMatch
                              from lockedByUser in lockedMatch.DefaultIfEmpty()
                              select new
                                {
                                    Entity = entity,
                                    CreatedByUser = createdByUser != null ? $"{createdByUser.FirstName} {createdByUser.LastName}" : string.Empty,
                                    ModifiedByUser = modifiedByUser != null ? $"{modifiedByUser.FirstName} {modifiedByUser.LastName}" : string.Empty,
                                    lockedByUser = lockedByUser != null ? $"{lockedByUser.FirstName} {lockedByUser.LastName}" : string.Empty,
                                };

            foreach (var userInfo in information)
            {
                userInfo.Entity.ModifiedBy = userInfo.ModifiedByUser;
                userInfo.Entity.CreatedBy = userInfo.CreatedByUser;
                userInfo.Entity.LockedByUsername = userInfo.lockedByUser;
            }
        }

        public async Task FillUserNameAsync(IProvideQuicklisterUserInfo userInfo)
        {
            if (userInfo is null)
            {
                return;
            }

            await this.FillUsersNameAsync(new List<IProvideQuicklisterUserInfo> { userInfo });
        }

        public async Task<IEnumerable<Response.User>> GetUsersById(IEnumerable<Guid> userIds)
        {
            var users = new List<Response.User>();

            if (!userIds.Any())
            {
                return users;
            }

            var idsNotInCache = new List<Guid>();
            foreach (Guid userId in userIds)
            {
                if (this.cache.Contains(userId.ToString()))
                {
                    var user = this.cache.Get(userId.ToString());
                    users.Add(user as Response.User);
                }
                else
                {
                    idsNotInCache.Add(userId);
                }
            }

            if (!idsNotInCache.Any())
            {
                return users;
            }

            var usersNotInCache = await this.serviceSubscriptionClient.User.GetAsync(new UserRequest() { Ids = idsNotInCache, Top = 100 });
            foreach (Response.User user in usersNotInCache.Data)
            {
                this.cache.Insert(user.Id.ToString(), user, 3600);
                users.Add(user);
            }

            return users;
        }
    }
}
