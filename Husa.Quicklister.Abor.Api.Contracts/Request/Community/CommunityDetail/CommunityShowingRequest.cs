namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingRequest
    {
        public string OccupantPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ShowingInstructions { get; set; }
        public string Directions { get; set; }
        public string OwnerName { get; set; }
        public ICollection<LockBoxType> LockBoxType { get; set; }
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
