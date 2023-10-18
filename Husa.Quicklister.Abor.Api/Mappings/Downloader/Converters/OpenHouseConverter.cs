namespace Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Husa.Downloader.CTX.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class OpenHouseConverter : ITypeConverter<ResidentialOpenHousesMessage, OpenHouseDto>
    {
        public OpenHouseDto Convert(ResidentialOpenHousesMessage source, OpenHouseDto destination, ResolutionContext context)
        {
            return new OpenHouseDto
            {
                StartTime = source.StartTime.Value.TimeOfDay,
                EndTime = source.EndTime.Value.TimeOfDay,
                Type = (OpenHouseType)Enum.Parse(typeof(OpenHouseType), source.StartTime.Value.ToString("dddd", ApplicationOptions.ApplicationCultureInfo)),
                Refreshments = source.Refreshments.Split(",").Select(x => x.GetEnumValueFromDescription<Refreshments>()).ToList(),
            };
        }
    }
}
