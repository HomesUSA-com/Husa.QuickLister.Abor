namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingDto
    {
        public string OccupantPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ShowingInstructions { get; set; }
        public string Directions { get; set; }
        public ICollection<LockBoxType> LockBoxType { get; set; }
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
