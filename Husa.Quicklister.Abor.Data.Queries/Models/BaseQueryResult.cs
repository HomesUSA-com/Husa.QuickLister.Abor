namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public class BaseQueryResult : IProvideQuicklisterUserInfo
    {
        public Guid Id { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByEmail { get; set; }

        public string LockedByUsername { get; set; }

        public Guid? LockedBy { get; set; }

        public string Directions { get; set; }

        public string OfficePhone { get; set; }

        public string BackupPhone { get; set; }

        public string PlanName { get; set; }

        public string OwnerName { get; set; }
    }
}
