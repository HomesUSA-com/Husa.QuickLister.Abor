namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class PublishInfo : ValueObject
    {
        public PublishInfo(ActionType publishType, Guid publishUser, MarketStatuses publishStatus)
        {
            this.PublishType = publishType;
            this.PublishUser = publishUser;
            this.PublishStatus = publishStatus;
            this.PublishDate = DateTime.UtcNow;
        }

        public PublishInfo()
        {
        }

        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.PublishType;
            yield return this.PublishUser;
            yield return this.PublishDate;
            yield return this.PublishStatus;
        }
    }
}
