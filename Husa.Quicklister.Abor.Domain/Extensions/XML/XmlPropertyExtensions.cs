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

            if (!xmlListing.Day.HasValue)
            {
                return;
            }

            if (ignoreRequestByCompletionDate && fields.ConstructionStage.HasValue && fields.ConstructionStage.Value == Enums.Domain.ConstructionStage.Complete)
            {
                return;
            }

            if (fields.ConstructionCompletionDate.HasValue)
            {
                if (fields.ConstructionCompletionDate.Value.Date != xmlListing.Day.Value.Date)
                {
                    fields.ConstructionCompletionDate = xmlListing.Day;
                    fields.ConstructionStage = xmlListing.Day.Value.Date > DateTime.UtcNow.Date ? Enums.Domain.ConstructionStage.Incomplete : Enums.Domain.ConstructionStage.Complete;
                }
                else if (fields.ConstructionCompletionDate.Value.Date <= DateTime.UtcNow.Date)
                {
                    fields.ConstructionStage = Enums.Domain.ConstructionStage.Complete;
                }
            }
        }
    }
}
