namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public abstract class Room : Entity, IProvideType, IProvideRoomInfo
    {
        protected Room(RoomType roomType, RoomLevel level, ICollection<RoomFeatures> features)
        {
            this.RoomType = roomType;
            this.Level = level;
            this.Features = features;
        }

        protected Room()
        {
        }

        public RoomType RoomType { get; set; }

        public RoomLevel Level { get; set; }

        public string EntityOwnerType { get; protected set; }

        public string FieldType => this.RoomType.ToString();
        public ICollection<RoomFeatures> Features { get; set; }
        public string CustomString()
        {
            return $"Level: {this.Level}";
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();
    }
}
