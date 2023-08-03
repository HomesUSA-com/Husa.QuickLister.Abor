namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideShowingInfo
    {
        string OccupantPhone { get; set; }
        string ContactPhone { get; set; }
        ICollection<ShowingInstructions> ShowingInstructions { get; set; }
        string Directions { get; set; }
        LockBoxType? LockBoxType { get; set; }
        ShowingRequirements? ShowingRequirements { get; set; }
    }
}
