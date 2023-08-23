namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideShowingInfo
    {
        string OccupantPhone { get; set; }
        string ContactPhone { get; set; }
        string ShowingInstructions { get; set; }
        string Directions { get; set; }
        LockBoxType? LockBoxType { get; set; }
        ShowingRequirements? ShowingRequirements { get; set; }
    }
}
