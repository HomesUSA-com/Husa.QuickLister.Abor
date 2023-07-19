namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingRequest
    {
        private string agentListApptPhone;

        public string AltPhoneCommunity { get; set; }

        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentListApptPhone
        {
            get { return this.agentListApptPhone; }
            set { this.agentListApptPhone = string.IsNullOrEmpty(value) ? value : Regex.Replace(value, "[^0-9]", string.Empty); }
        }

        public Showing? Showing { get; set; }

        public string RealtorContactEmail { get; set; }

        [MaxLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }

        [MaxLength(1024, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarks { get; set; }

        public bool EnableOpenHouses { get; set; }

        public bool OpenHousesAgree { get; set; }

        public bool ShowOpenHousesPending { get; set; }
    }
}
