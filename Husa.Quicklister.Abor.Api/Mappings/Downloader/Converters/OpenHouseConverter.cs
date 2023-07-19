namespace Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters
{
    using System;
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class OpenHouseConverter : ITypeConverter<OpenHouseMessage, OpenHouseDto>
    {
        public OpenHouseDto Convert(OpenHouseMessage source, OpenHouseDto destination, ResolutionContext context)
        {
            return new OpenHouseDto
            {
                StartTime = source.StartTime.TimeOfDay,
                EndTime = source.EndTime.TimeOfDay,
                Lunch = source.Lunch,
                Refreshments = source.Refreshments,
                Type = (OpenHouseType)Enum.Parse(typeof(OpenHouseType), source.StartTime.ToString("dddd", ApplicationOptions.ApplicationCultureInfo)),
            };
        }
    }
}
