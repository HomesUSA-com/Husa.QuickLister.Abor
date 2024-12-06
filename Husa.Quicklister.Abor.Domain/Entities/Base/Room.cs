namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class Room : Entity, IProvideType, IProvideRoomInfo
    {
        public Room(RoomType roomType, RoomLevel level, ICollection<RoomFeatures> features, string description)
            : this()
        {
            this.RoomType = roomType;
            this.Level = level;
            this.Features = features;
            this.Description = description;
        }

        protected Room()
            : base()
        {
        }

        public RoomType RoomType { get; set; }

        public RoomLevel Level { get; set; }

        public string EntityOwnerType { get; protected set; }
        public string Description { get; set; }

        public string FieldType => this.RoomType.ToString();
        public ICollection<RoomFeatures> Features { get; set; }
        public string CustomString()
        {
            return $"Level: {this.Level}";
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.Level;
            yield return this.RoomType;
            yield return this.EntityOwnerType;
            yield return this.Features;
            yield return this.Description;
        }
    }
}
