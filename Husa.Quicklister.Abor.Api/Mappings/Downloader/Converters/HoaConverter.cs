namespace Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters
{
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class HoaConverter : ITypeConverter<HoaMessage, HoaDto>
    {
        public HoaDto Convert(HoaMessage source, HoaDto destination, ResolutionContext context) => new HoaDto
        {
            Name = source.Name,
            TransferFee = source.TransferFee,
            Fee = source.Fee,
            BillingFrequency = !string.IsNullOrEmpty(source.BillingFrequency) ?
                    source.BillingFrequency.ToEnumFromEnumMember<BillingFrequency>() :
                    BillingFrequency.Annually,
            Website = source.Website,
            ContactPhone = source.ContactPhone,
            HoaType = source.EntityOwnerType,
        };
    }
}
