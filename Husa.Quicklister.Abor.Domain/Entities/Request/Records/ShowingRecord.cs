namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record ShowingRecord : IProvideSummary
    {
        public const string SummarySection = "Showing";

        [MaxLength(1000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarks { get; set; }

        public string OccupantPhone { get; set; }

        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string ContactPhone { get; set; }

        public string ShowingInstructions { get; set; }

        public ICollection<string> RealtorContactEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(2000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }
        public string OwnerName { get; set; }

        [MaxLength(1000, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarksAdditional { get; set; }
        public string LockBoxSerialNumber { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }

        public LockBoxType LockBoxType { get; set; }
        public bool EnableOpenHouses { get; set; }
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
                ShowOpenHousesPending = showingInfo.ShowOpenHousesPending,
                LockBoxType = showingInfo.LockBoxType ?? throw new DomainException(nameof(showingInfo.LockBoxType)),
                ShowingRequirements = showingInfo.ShowingRequirements,
                OwnerName = showingInfo.OwnerName,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
