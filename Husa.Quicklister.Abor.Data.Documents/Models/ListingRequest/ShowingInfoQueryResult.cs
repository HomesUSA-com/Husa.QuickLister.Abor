namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingInfoQueryResult
    {
        public string AgentPrivateRemarks { get; set; }

        public string OccupantPhone { get; set; }

        public string ContactPhone { get; set; }

        public ICollection<ShowingInstructions> ShowingInstructions { get; set; }

        public string RealtorContactEmail { get; set; }

        public string Directions { get; set; }

        public bool EnableOpenHouses { get; set; }

        public bool OpenHousesAgree { get; set; }

        public bool ShowOpenHousesPending { get; set; }
        public LockBoxType? LockBoxType { get; set; }
        public ShowingRequirements? ShowingRequirements { get; set; }
    }
}
