namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public abstract class Room : Entity, IProvideType, IProvideRoomInfo
    {
        protected Room(RoomType roomType, RoomLevel level, int width, int length, ICollection<RoomFeatures> features)
        {
            this.RoomType = roomType;
            this.Level = level;
            this.Width = width;
            this.Length = length;
            this.Features = features;
        }

        protected Room()
        {
        }

        public int Length { get; set; }

        public int Width { get; set; }

        public RoomLevel Level { get; set; }

        public RoomType RoomType { get; set; }

        public string EntityOwnerType { get; protected set; }

        public ICollection<RoomFeatures> Features { get; set; }

        public string FieldType => this.RoomType.ToString();

        public string CustomString()
        {
            return $"Level: {this.Level}, Dimensions: {this.Length} x {this.Width}";
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();
    }
}
