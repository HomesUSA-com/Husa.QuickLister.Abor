namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;

    public class XmlManagementResponse
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime SysCreatedOn { get; set; }

        public virtual Guid? SysCreatedBy { get; set; }

        public virtual Guid? SysModifiedBy { get; set; }

        public Guid? LockedBy { get; set; }

        public Guid ListingSaleId { get; set; }

        public bool IsManuallyManaged { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string LockedByUsername { get; set; }
    }
}
