namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleDto
    {
        public MarketStatuses MlsStatus { get; set; }

        public string StreetName { get; set; }

        public string StreetNumber { get; set; }

        public Cities City { get; set; }

        public States State { get; set; }

        public decimal ListPrice { get; set; }

        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public DateTime? ConstructionCompletionDate { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? CommunityId { get; set; }

        public Guid? PlanId { get; set; }

        public Guid? ListingIdToImport { get; set; }

        public bool IsManuallyManaged { get; set; }
    }
}
