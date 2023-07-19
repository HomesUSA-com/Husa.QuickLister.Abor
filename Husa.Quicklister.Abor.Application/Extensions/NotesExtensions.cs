namespace Husa.Quicklister.Abor.Application.Extensions
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using RemoteNoteType = Husa.Notes.Domain.Enums.NoteType;

    public static class NotesExtensions
    {
        public static RemoteNoteType ToRemoteNoteType(this NoteType remoteNoteType) => remoteNoteType switch
        {
            NoteType.Residential => RemoteNoteType.Residential,
            NoteType.CommunityProfile => RemoteNoteType.CommunityProfile,
            NoteType.PlanProfile => RemoteNoteType.PlanProfile,
            NoteType.Lot => RemoteNoteType.Lot,
            NoteType.Lease => RemoteNoteType.Lease,
            NoteType.ListingRequest => RemoteNoteType.ListingRequest,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteNoteType), $"The note type {remoteNoteType} is not recognized"),
        };

        public static NoteType ToLocalNoteType(this RemoteNoteType remoteNoteType) => remoteNoteType switch
        {
            RemoteNoteType.Residential => NoteType.Residential,
            RemoteNoteType.CommunityProfile => NoteType.CommunityProfile,
            RemoteNoteType.PlanProfile => NoteType.PlanProfile,
            RemoteNoteType.Lot => NoteType.Lot,
            RemoteNoteType.Lease => NoteType.Lease,
            RemoteNoteType.ListingRequest => NoteType.ListingRequest,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteNoteType), $"The note type {remoteNoteType} is not recognized"),
        };
    }
}
