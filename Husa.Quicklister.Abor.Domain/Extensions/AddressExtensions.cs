namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public static class AddressExtensions
    {
        public static string GetFormalAddress<TAddress>(this TAddress address)
            where TAddress : IProvideAddress
        {
            return $"{address.StreetNumber} {address.StreetName}, {GetReadableCity(address.City)} {address.ZipCode}";
        }

        private static string GetReadableCity(Cities? city) => Enum.IsDefined(typeof(Cities), city) ? city.GetEnumDescription() : null;
    }
}
