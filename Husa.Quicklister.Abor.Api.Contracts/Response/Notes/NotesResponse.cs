namespace Husa.Quicklister.Abor.Api.Contracts.Response.Notes
{
    using System;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class NotesResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string LockedByUsername { get; set; }

        public Guid? LockedBy { get; set; }

        public NoteType Type { get; set; }
    }
}
