namespace Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters
{
    using System;
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
            };
        }

        private static string GetRoomType(string roomTypeSource) => roomTypeSource switch
        {
            "BEDROO" => "Bedroom",
            "LIVROO" => "Living",
            "MASBED" => "PrimaryBedroom",
            "MASBATH" => "PrimaryBathroom",
            "HALBAT" => "Bathroom",
            "KITCHE" => "Kitchen",
            "DINROO" => "Dining",
            "GAMROO" => "Game",
            "OFFICE" => "Office",
            _ => "Bedroom",
        };
    }
}
