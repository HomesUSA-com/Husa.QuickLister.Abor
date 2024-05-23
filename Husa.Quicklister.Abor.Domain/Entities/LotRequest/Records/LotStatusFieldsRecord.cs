namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Request;

    public record LotStatusFieldsRecord : StatusFieldsRecord
    {
        public LotStatusFieldsRecord CloneRecord() => (LotStatusFieldsRecord)this.MemberwiseClone();

        public static LotStatusFieldsRecord CreateRecord(ListingStatusFieldsInfo statusFieldInfo) =>
            StatusFieldsRecord.CreateRecord<ListingStatusFieldsInfo, LotStatusFieldsRecord>(statusFieldInfo);

        public virtual void UpdateInformation(ListingStatusFieldsInfo statusFieldInfo) =>
            this.UpdateInformation<ListingStatusFieldsInfo>(statusFieldInfo);
    }
}
