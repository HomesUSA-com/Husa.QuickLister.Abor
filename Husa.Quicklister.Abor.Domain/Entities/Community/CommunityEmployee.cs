namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using ExtensionsEntities = Husa.Quicklister.Extensions.Domain.Entities.Community;

    public class CommunityEmployee : ExtensionsEntities.CommunityEmployee
    {
        public CommunityEmployee(Guid userId, Guid communityId, Guid companyId)
            : base(userId, communityId, companyId)
        {
        }

        public virtual CommunitySale Community { get; set; }
    }
}
