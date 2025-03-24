namespace Husa.Quicklister.Abor.Domain.Tests.Providers
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Moq;

    public static class TestProviderListing
    {
        public static SaleListing CreateSaleListing(MarketStatuses status = MarketStatuses.Active)
        {
            var saleListing = new Mock<SaleListing>();

            saleListing.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            saleListing.SetupGet(x => x.MlsStatus).Returns(status);
            saleListing.SetupGet(x => x.MlsNumber).Returns("12345");
            saleListing.SetupGet(x => x.ListPrice).Returns(250000);
            saleListing.SetupGet(x => x.ListDate).Returns(DateTime.UtcNow);
            saleListing.SetupGet(x => x.ExpirationDate).Returns(DateTime.UtcNow.AddMonths(6));
            saleListing.SetupGet(x => x.CompanyId).Returns(Guid.NewGuid());

            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            saleProperty.SetupGet(x => x.CompanyId).Returns(Guid.NewGuid());
            saleProperty.SetupGet(x => x.CommunityId).Returns(Guid.NewGuid());
            saleProperty.SetupGet(x => x.PlanId).Returns(Guid.NewGuid());
            saleProperty.SetupGet(x => x.SysCreatedOn).Returns(DateTime.UtcNow);
            saleProperty.SetupGet(x => x.SysTimestamp).Returns(DateTime.UtcNow);
            saleProperty.SetupGet(x => x.SysCreatedBy).Returns(Guid.NewGuid());
            saleProperty.SetupGet(x => x.SysModifiedOn).Returns(DateTime.UtcNow);
            saleProperty.SetupGet(x => x.SysModifiedBy).Returns(Guid.NewGuid());

            saleListing.SetupGet(x => x.SaleProperty).Returns(saleProperty.Object);
            saleListing.SetupGet(x => x.SalePropertyId).Returns(saleProperty.Object.Id);

            var statusFieldsInfo = new Mock<ListingStatusFieldsInfo>();
            saleListing.SetupGet(x => x.StatusFieldsInfo).Returns(statusFieldsInfo.Object);
            saleListing.SetupGet(x => x.PublishInfo).Returns(new Mock<PublishInfo>().Object);

            return saleListing.Object;
        }
    }
}
