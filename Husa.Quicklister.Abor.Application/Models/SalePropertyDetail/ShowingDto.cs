namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingDto
    {
        public string AltPhoneCommunity { get; set; }
        public string AgentListApptPhone { get; set; }
        public Showing? Showing { get; set; }
        public string RealtorContactEmail { get; set; }
        public string Directions { get; set; }
        public string AgentPrivateRemarks { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool OpenHousesAgree { get; set; }
        public bool ShowOpenHousesPending { get; set; }
    }
}
