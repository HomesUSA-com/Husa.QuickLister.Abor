namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public class CompletedHomesWithoutPRQueryResult : IProvideQuicklisterUserInfo
    {
        public Guid Id { get; set; }

        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Address { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ConstructionCompletionDate { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public string CreatedBy { get; set; }

        public string LockedByUsername { get; set; }

        public Guid? LockedBy { get; set; }
    }
}
