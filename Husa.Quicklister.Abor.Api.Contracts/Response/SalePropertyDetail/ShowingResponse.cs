namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingResponse
    {
        public string OccupantPhone { get; set; }
        public string ContactPhone { get; set; }

        public string ShowingInstructions { get; set; }
        public string RealtorContactEmail { get; set; }
        public string Directions { get; set; }
        public string AgentPrivateRemarks { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool OpenHousesAgree { get; set; }
        public bool ShowOpenHousesPending { get; set; }
        public LockBoxType? LockBoxType { get; set; }
        public ShowingRequirements? ShowingRequirements { get; set; }
    }
}
