namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record ShowingRecord : IProvideSummary
    {
        public const string SummarySection = "Showing";

        [MaxLength(1024, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarks { get; set; }

        public string OccupantPhone { get; set; }

        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string ContactPhone { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ShowingInstructions { get; set; }

        public string RealtorContactEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }
        public string OwnerName { get; set; }
        public string AgentPrivateRemarksAdditional { get; set; }
        public string LockBoxSerialNumber { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<LockBoxType> LockBoxType { get; set; }
        public bool EnableOpenHouses { get; set; }
        public bool OpenHousesAgree { get; set; }
        public bool ShowOpenHousesPending { get; set; }

        public ShowingRecord CloneRecord() => (ShowingRecord)this.MemberwiseClone();
        public static ShowingRecord CreateRecord(ShowingInfo showingInfo)
        {
            if (showingInfo == null)
            {
                return new();
            }

            return new()
            {
                OccupantPhone = showingInfo.OccupantPhone,
                ContactPhone = showingInfo.ContactPhone,
                AgentPrivateRemarks = showingInfo.AgentPrivateRemarks,
                AgentPrivateRemarksAdditional = showingInfo.AgentPrivateRemarksAdditional,
                LockBoxSerialNumber = showingInfo.LockBoxSerialNumber,
                ShowingInstructions = showingInfo.ShowingInstructions,
                RealtorContactEmail = showingInfo.RealtorContactEmail,
                Directions = showingInfo.Directions,
                EnableOpenHouses = showingInfo.EnableOpenHouses,
                OpenHousesAgree = showingInfo.OpenHousesAgree,
                ShowOpenHousesPending = showingInfo.ShowOpenHousesPending,
                LockBoxType = showingInfo.LockBoxType,
                ShowingRequirements = showingInfo.ShowingRequirements,
                OwnerName = showingInfo.OwnerName,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
