namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using System.Text.RegularExpressions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingResponse
    {
        private string agentListApptPhone;

        public string AltPhoneCommunity { get; set; }
        public string AgentListApptPhone
        {
            get { return string.IsNullOrEmpty(this.agentListApptPhone) ? this.agentListApptPhone : Regex.Replace(this.agentListApptPhone, "[^0-9]", string.Empty); }
            set { this.agentListApptPhone = value; }
        }

        public Showing? Showing { get; set; }
        public string RealtorContactEmail { get; set; }
        public string Directions { get; set; }
        public string AgentPrivateRemarks { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool OpenHousesAgree { get; set; }
        public bool ShowOpenHousesPending { get; set; }
    }
}
