namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingDto
    {
        public string OccupantPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ShowingInstructions { get; set; }
        public string Directions { get; set; }
        public LockBoxType? LockBoxType { get; set; }
        public ShowingRequirements? ShowingRequirements { get; set; }
    }
}
