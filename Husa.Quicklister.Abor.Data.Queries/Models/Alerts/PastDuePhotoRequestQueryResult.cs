namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;
    using Husa.Extensions.Common.Enums;

    public class PastDuePhotoRequestQueryResult
    {
        public Guid ListingId { get; set; }

        public Guid PhotoRequestId { get; set; }

        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public string Address { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public DateTime? AssignedOn { get; set; }

        public string AssignedTo { get; set; }

        public DateTime? ContactDate { get; set; }

        public DateTime? ScheduleDate { get; set; }
    }
}
