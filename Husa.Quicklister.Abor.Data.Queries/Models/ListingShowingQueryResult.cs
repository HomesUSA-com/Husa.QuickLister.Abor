namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;

    public class ListingShowingQueryResult : ShowingQueryResult
    {
        public string AgentPrivateRemarks { get; set; }
        public string AgentPrivateRemarksAdditional { get; set; }
        public string LockBoxSerialNumber { get; set; }
        public string RealtorContactEmail { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool OpenHousesAgree { get; set; }
        public bool ShowOpenHousesPending { get; set; }
    }
}
