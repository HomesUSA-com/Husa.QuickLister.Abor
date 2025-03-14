namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using System;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlPropertyExtensions
    {
        public static void UpdateFromXml(this IProvideSaleProperty fields, XmlListingDetailResponse xmlListing, bool ignoreRequestByCompletionDate = false)
        {
            ArgumentNullException.ThrowIfNull(xmlListing);
            if (!string.IsNullOrEmpty(xmlListing.LegalDescLot))
            {
                fields.LotDescription = xmlListing.LegalDescLot.CsvToEnum<LotDescription>().ToArray();
            }

            if (ignoreRequestByCompletionDate && fields.ConstructionStage.HasValue && fields.ConstructionStage.Value == ConstructionStage.Complete)
            {
                return;
            }

            if (xmlListing.Day.HasValue && fields.ConstructionCompletionDate.HasValue && fields.ConstructionCompletionDate.Value.Date != xmlListing.Day.Value.Date)
            {
                fields.ConstructionCompletionDate = xmlListing.Day;
                fields.ConstructionStage = xmlListing.Day.Value.Date > DateTime.UtcNow.Date ? ConstructionStage.Incomplete : ConstructionStage.Complete;
            }
        }
    }
}
