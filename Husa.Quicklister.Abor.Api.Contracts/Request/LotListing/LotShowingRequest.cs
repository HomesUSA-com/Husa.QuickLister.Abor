namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingRequest
    {
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        public string ApptPhone { get; set; }
        public string ShowingServicePhone { get; set; }
        public string OwnerName { get; set; }
        public string ShowingInstructions { get; set; }
        public string PublicRemarks { get; set; }
        public string Directions { get; set; }
        public ICollection<ShowingContactType> ShowingContactType { get; set; }
        public string ShowingContactName { get; set; }
    }
}
