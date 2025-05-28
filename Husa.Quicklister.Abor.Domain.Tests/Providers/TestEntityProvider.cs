namespace Husa.Quicklister.Abor.Domain.Tests.Providers
{
    using System;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;

    public static class TestEntityProvider
    {
        public static IUserContextProvider GetIUserContextProvider()
        {
            var userContextProvider = new Mock<IUserContextProvider>();
            var userId = Guid.NewGuid();
            userContextProvider.Setup(u => u.GetCurrentUserId()).Returns(userId).Verifiable();
            userContextProvider.Setup(u => u.GetUserLocalDate()).Returns(DateTime.UtcNow).Verifiable();
            return userContextProvider.Object;
        }

        public static Domain.Entities.Listing.SaleListing GetSaleListing(Guid id, Guid? planId = null, Guid? communityId = null, MarketStatuses? mlsStatus = null)
        {
            var localLockStatus = LockedStatus.NoLocked;
            var localMlsStatus = mlsStatus ?? MarketStatuses.Active;
            var saleProperty = new Mock<SaleProperty>();

            if (communityId.HasValue)
            {
                saleProperty.Setup(x => x.CommunityId).Returns(communityId.Value);
            }

            if (planId.HasValue)
            {
                saleProperty.Setup(x => x.PlanId).Returns(planId.Value);
            }

            var list = new Mock<Domain.Entities.Listing.SaleListing>();
            list.Setup(x => x.Id).Returns(id);
            list.Setup(x => x.MlsNumber).Returns($"{localLockStatus}-1234");
            list.Setup(x => x.LockedStatus).Returns(localLockStatus);
            list.Setup(x => x.IsDeleted).Returns(false);
            list.Setup(x => x.MlsStatus).Returns(localMlsStatus);
            list.Setup(x => x.SaleProperty).Returns(saleProperty.Object);
            return list.Object;
        }
    }
}
