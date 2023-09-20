namespace Husa.Quicklister.Abor.Domain.Entities.OpenHouse
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using AborIProvideOpenHouseInfo = Husa.Quicklister.Abor.Domain.Interfaces.IProvideOpenHouseInfo;
    using OpenHouseTypeEnum = Husa.Quicklister.Extensions.Domain.Enums.OpenHouseType;

    public class OpenHouse : Entity, IProvideType, AborIProvideOpenHouseInfo
    {
        protected OpenHouse(
          OpenHouseTypeEnum type,
          TimeSpan startTime,
          TimeSpan endTime,
          ICollection<Refreshments> refreshments)
        {
            this.Type = type;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Refreshments = refreshments;
        }

        protected OpenHouse()
        {
        }

        public OpenHouseTypeEnum Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public ICollection<Refreshments> Refreshments { get; set; }

        public string OpenHouseType { get; protected set; }

        public string FieldType => this.Type.ToString();

        public string CustomString()
        {
            return $"StartTime: {this.StartTime} , EndTime: {this.EndTime}, Refreshments : {this.Refreshments}";
        }

        protected static OpenHouseTypeEnum GetOpenHouseType(DayOfWeek day) => day switch
        {
            DayOfWeek.Monday => OpenHouseTypeEnum.Monday,
            DayOfWeek.Wednesday => OpenHouseTypeEnum.Wednesday,
            DayOfWeek.Thursday => OpenHouseTypeEnum.Thursday,
            DayOfWeek.Friday => OpenHouseTypeEnum.Friday,
            DayOfWeek.Saturday => OpenHouseTypeEnum.Saturday,
            DayOfWeek.Sunday => OpenHouseTypeEnum.Sunday,
            _ => OpenHouseTypeEnum.Tuesday,
        };

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.Refreshments;
            yield return this.StartTime;
            yield return this.EndTime;
            yield return this.Type;
        }
    }
}
