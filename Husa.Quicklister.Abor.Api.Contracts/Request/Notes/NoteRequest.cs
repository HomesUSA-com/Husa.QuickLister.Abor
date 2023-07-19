namespace Husa.Quicklister.Abor.Api.Contracts.Request.Notes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NoteRequest
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
