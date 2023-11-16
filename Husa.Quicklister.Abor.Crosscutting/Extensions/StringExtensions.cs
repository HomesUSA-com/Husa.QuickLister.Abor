namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    public static class StringExtensions
    {
        public static decimal ConvertToDecimal(this string value)
        {
            var result = decimal.TryParse(value, out decimal outParameter) ? outParameter : 0;
            return result;
        }

        public static string GetSubstring(this string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }
    }
}
