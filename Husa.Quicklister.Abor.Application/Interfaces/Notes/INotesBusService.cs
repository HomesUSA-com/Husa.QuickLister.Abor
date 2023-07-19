namespace Husa.Quicklister.Abor.Application.Interfaces.Notes
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface INotesBusService
    {
        Task CreateAsync(Guid entityId, string title, string description, NoteType noteType);

        Task UpdateAsync(Guid entityId, Guid noteId, string title, string description, NoteType noteType);

        Task DeleteByIdAsync(Guid noteId);
    }
}
