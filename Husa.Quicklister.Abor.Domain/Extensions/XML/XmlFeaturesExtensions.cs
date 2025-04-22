namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using System.Net;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;
    using Husa.Quicklister.Extensions.Crosscutting.Extensions;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlFeaturesExtensions
    {
        public const int PropertyDescriptionLength = 4000;
        private static readonly string RemoveKeyword = "MLS Number";
        public static void UpdateFromXml(this IProvideSaleFeature fields, XmlListingDetailResponse xmlListing)
        {
            if (!string.IsNullOrEmpty(xmlListing.Description))
            {
                var newDescription = CleanPropertyDescription(xmlListing.Description, forComparisonOnly: true);
                var oldDescription = CleanPropertyDescription(fields.PropertyDescription, forComparisonOnly: true);
                if (newDescription != oldDescription)
                {
                    fields.PropertyDescription = CleanPropertyDescription(xmlListing.Description);
                }
            }
        }

        private static string CleanPropertyDescription(string description, bool forComparisonOnly = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                return description;
            }

            var decoded = WebUtility.HtmlDecode(description);
            decoded = WebUtility.HtmlDecode(decoded);
            decoded = decoded
                .Replace("<br>", " ")
                .ReplaceLineEndings(" ")
                .CleanAfterKeyword(RemoveKeyword)
                .GetSubstring(PropertyDescriptionLength);
            while (decoded.Contains("  "))
            {
                decoded = decoded.Replace("  ", " ");
            }

            decoded = decoded.Trim();

            if (forComparisonOnly)
            {
                decoded = decoded.CleanForComparison();
            }

            return decoded;
        }
    }
}
