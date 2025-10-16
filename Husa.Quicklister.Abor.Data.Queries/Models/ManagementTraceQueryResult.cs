namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public class ManagementTraceQueryResult : IProvideQuicklisterUserInfo
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime SysCreatedOn { get; set; }

        public virtual Guid? SysCreatedBy { get; set; }

        public virtual Guid? SysModifiedBy { get; set; }

        public Guid? LockedBy { get; set; }

        public Guid ListingSaleId { get; set; }

        public bool IsManuallyManaged { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByEmail { get; set; }

        public string ModifiedBy { get; set; }

        public string LockedByUsername { get; set; }
    }
}
