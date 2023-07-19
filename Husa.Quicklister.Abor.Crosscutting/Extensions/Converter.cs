namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    public static class Converter
    {
#nullable enable
        public static int? FireplacesToIntConverter(this string? fireplaces)
        {
            if (fireplaces is null)
            {
                return null;
            }

            return fireplaces.Equals("3+") ? 3 : int.Parse(fireplaces);
        }

        public static int? ConstructionStartYearToIntConverter(this string? constructionStartYear)
        {
            if (constructionStartYear is null)
            {
                return null;
            }

            return constructionStartYear.Equals("U") ? null : int.Parse(constructionStartYear);
        }
    }
}
