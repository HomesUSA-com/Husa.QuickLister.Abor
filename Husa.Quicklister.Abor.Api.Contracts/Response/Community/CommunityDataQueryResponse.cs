namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityDataQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Builder { get; set; }

        public Cities City { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public MarketCode Market { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string Directions { get; set; }

        public string OfficePhone { get; set; }

        public string BackupPhone { get; set; }
    }
}
