namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingRequest
    {
        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string OccupantPhone { get; set; }

        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string ContactPhone { get; set; }

        public string ShowingInstructions { get; set; }

        public ICollection<string> RealtorContactEmail { get; set; }

        [MaxLength(2000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }

        [MaxLength(2000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarks { get; set; }
        public string OwnerName { get; set; }

        [MaxLength(1000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarksAdditional { get; set; }
        public string LockBoxSerialNumber { get; set; }
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        public LockBoxType? LockBoxType { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool ShowOpenHousesPending { get; set; }
    }
}
