namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class SaleListingTrace : Entity
    {
        public Guid ListingSaleId { get; set; }

        public Guid? ListingSaleRequestId { get; set; }

        public ActionType ActionType { get; set; }

        public MarketStatuses RequestMlsStatus { get; set; }

        public virtual SaleListing ListingSale { get; set; }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.ListingSaleId;
            yield return this.ListingSaleRequestId;
            yield return this.ActionType;
            yield return this.RequestMlsStatus;
        }
    }
}
