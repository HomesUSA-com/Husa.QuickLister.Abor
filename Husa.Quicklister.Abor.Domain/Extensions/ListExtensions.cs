namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static void AddValue<T>(this List<T> requiredFields, bool condition, T value)
        {
            if (condition)
            {
                requiredFields.Add(value);
            }
        }
    }
}
