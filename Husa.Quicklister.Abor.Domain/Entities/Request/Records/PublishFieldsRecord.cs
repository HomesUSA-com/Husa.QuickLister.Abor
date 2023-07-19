namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;

    public record PublishFieldsRecord
    {
        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }

        public PublishFieldsRecord CloneRecord() => (PublishFieldsRecord)this.MemberwiseClone();
        public static PublishFieldsRecord CreateRecord(PublishInfo statusFieldInfo)
        {
            if (statusFieldInfo is null)
            {
                return null;
            }

            return new()
            {
                PublishType = statusFieldInfo.PublishType,
                PublishUser = statusFieldInfo.PublishUser,
                PublishStatus = statusFieldInfo.PublishStatus,
                PublishDate = statusFieldInfo.PublishDate,
            };
        }
    }
}
