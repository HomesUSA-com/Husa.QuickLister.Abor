namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingResponse
    {
        public string OccupantPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ShowingInstructions { get; set; }
        public string Directions { get; set; }
        public LockBoxType? LockBoxType { get; set; }
        public ShowingRequirements? ShowingRequirements { get; set; }
    }
}
