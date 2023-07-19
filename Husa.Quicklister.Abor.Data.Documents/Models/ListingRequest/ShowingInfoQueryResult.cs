namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingInfoQueryResult
    {
        public string AgentPrivateRemarks { get; set; }

        public string AltPhoneCommunity { get; set; }

        public string AgentListApptPhone { get; set; }

        public Showing Showing { get; set; }

        public string RealtorContactEmail { get; set; }

        public string Directions { get; set; }

        public bool EnableOpenHouses { get; set; }

        public bool OpenHousesAgree { get; set; }

        public bool ShowOpenHousesPending { get; set; }
    }
}
