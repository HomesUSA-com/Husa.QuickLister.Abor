namespace Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class RoomConverter : ITypeConverter<RoomMessage, RoomDto>
    {
        public RoomDto Convert(RoomMessage source, RoomDto destination, ResolutionContext context)
        {
            return new RoomDto
            {
                RoomType = (RoomType)Enum.Parse(typeof(RoomType), GetRoomType(source.Type)),
                Level = source.Level.ToEnumFromEnumMember<RoomLevel>(),
                Length = !string.IsNullOrEmpty(source.Length) ? int.Parse(source.Length) : 0,
                Width = !string.IsNullOrEmpty(source.Width) ? int.Parse(source.Width) : 0,
                Features = source.Features.CsvToEnum<RoomFeatures>().ToArray(),
            };
        }

        private static string GetRoomType(string roomTypeSource) => roomTypeSource switch
        {
            "BEDROO" => "Bed",
            "BREROO" => "Breakfast",
            "DINROO" => "Dining",
            "FULBAT" => "FullBath",
            "GAMROO" => "Game",
            "HALBAT" => "HalfBath",
            "KITCHE" => "Kitchen",
            "STUDY" => "Study",
            "LIVROO" => "Living",
            "MASBED" => "MasterBedroom",
            "OFFICE" => "Office",
            "STUDEN" => "Studen",
            "UTIROO" => "Utility",
            "ENTRY" => "Entry",
            "FAMILY" => "Family",
            "MASBATH" => "MasterBath",
            "MASBEDCLO" => "MasterBedroomCloset",
            _ => "Other",
        };
    }
}
