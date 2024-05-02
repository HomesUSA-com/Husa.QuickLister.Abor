namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record RoomRecord : IProvideType, IProvideRoomInfo
    {
        public const string SummarySection = "Rooms";

        public Guid Id { get; set; }

        public Guid SalePropertyId { get; set; }

        public RoomLevel Level { get; set; }

        public RoomType RoomType { get; set; }

        public ICollection<RoomFeatures> Features { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysTimestamp { get; set; }

        public Guid CompanyId { get; set; }

        public string FieldType => this.RoomType.ToString();

        public RoomRecord CloneRecord() => (RoomRecord)this.MemberwiseClone();
        public static RoomRecord CreateRoom(ListingSaleRoom room)
        {
            if (room == null)
            {
                return new();
            }

            return new()
            {
                SalePropertyId = room.SalePropertyId,
                Level = room.Level,
                RoomType = room.RoomType,
                Features = room.Features,
                Id = room.Id,
                SysCreatedOn = room.SysCreatedOn,
                SysModifiedOn = room.SysModifiedOn,
                IsDeleted = room.IsDeleted,
                SysModifiedBy = room.SysModifiedBy,
                SysCreatedBy = room.SysCreatedBy,
                SysTimestamp = room.SysTimestamp,
                CompanyId = room.CompanyId,
            };
        }

        public string CustomString()
        {
            throw new NotImplementedException();
        }
    }
}
