namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    public static class StringExtensions
    {
        public static decimal ConvertToDecimal(this string value)
        {
            var result = decimal.TryParse(value, out decimal outParameter) ? outParameter : 0;
            return result;
        }
    }
}
