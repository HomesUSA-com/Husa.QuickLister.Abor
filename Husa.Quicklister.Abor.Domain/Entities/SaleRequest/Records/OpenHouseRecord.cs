namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using AborIProvideOpenHouseInfo = Husa.Quicklister.Abor.Domain.Interfaces.IProvideOpenHouseInfo;

    public record OpenHouseRecord : IProvideType, AborIProvideOpenHouseInfo
    {
        public const string SummarySection = "Open House";

        public Guid Id { get; set; }

        public OpenHouseType Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public ICollection<Refreshments> Refreshments { get; set; }

        public bool IsDeleted { get; set; }

        public static EntityType OpenHouseType => EntityType.SaleProperty;

        public string FieldType => this.Type.ToString();

        public OpenHouseRecord CloneRecord() => (OpenHouseRecord)this.MemberwiseClone();
        public static OpenHouseRecord CreateOpenHouse(SaleListingOpenHouse openHouse)
        {
            if (openHouse == null)
            {
                return new();
            }

            return new()
            {
                Id = openHouse.Id,
                Type = openHouse.Type,
                StartTime = openHouse.StartTime,
                EndTime = openHouse.EndTime,
                Refreshments = openHouse.Refreshments,
                IsDeleted = openHouse.IsDeleted,
            };
        }

        public string CustomString()
        {
            return $"StartTime: {this.StartTime} , EndTime: {this.EndTime}, Refreshments : {this.Refreshments}";
        }
    }
}
