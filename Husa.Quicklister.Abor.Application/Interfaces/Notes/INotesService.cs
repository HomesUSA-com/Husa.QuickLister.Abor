namespace Husa.Quicklister.Abor.Application.Interfaces.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models;

    public interface INotesService
    {
        Task<NoteDetailResult> GetNote(Guid entityId, Guid noteId);

        Task<IEnumerable<NoteDetailResult>> GetNotes(Guid entityId);

        Task UpdateNote(Guid entityId, Guid noteId, string title, string description);

        Task CreateNote(Guid entityId, string title, string description);

        Task DeleteNote(Guid entityId, Guid noteId);
    }
}
