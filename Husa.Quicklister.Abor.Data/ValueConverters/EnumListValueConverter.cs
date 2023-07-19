namespace Husa.Quicklister.Abor.Data.ValueConverters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    public class EnumListValueConverter<T> : ValueConverter<ICollection<T>, string>
        where T : Enum
    {
        public EnumListValueConverter()
            : base(
                  convertToProviderExpression: enumField => enumField.ToStringFromEnumMembers(true),
                  convertFromProviderExpression: enumField => enumField.CsvToEnum<T>(true).ToList())
        {
        }
    }
}
