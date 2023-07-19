namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record ShowingRecord : IProvideSummary
    {
        private string agentListApptPhone;

        public const string SummarySection = "Showing";

        [MaxLength(1024, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentPrivateRemarks { get; set; }

        public string AltPhoneCommunity { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(14, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string AgentListApptPhone
        {
            get { return this.agentListApptPhone; }
            set { this.agentListApptPhone = string.IsNullOrEmpty(value) ? value : Regex.Replace(value, "[^0-9]", string.Empty); }
        }

        [Required]
        public Showing Showing { get; set; }

        public string RealtorContactEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Directions { get; set; }

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
                AgentPrivateRemarks = showingInfo.AgentPrivateRemarks,
                AltPhoneCommunity = showingInfo.AltPhoneCommunity,
                AgentListApptPhone = showingInfo.AgentListApptPhone,
                Showing = showingInfo.Showing ?? throw new DomainException(nameof(showingInfo.Showing)),
                RealtorContactEmail = showingInfo.RealtorContactEmail,
                Directions = showingInfo.Directions,
                EnableOpenHouses = showingInfo.EnableOpenHouses,
                OpenHousesAgree = showingInfo.OpenHousesAgree,
                ShowOpenHousesPending = showingInfo.ShowOpenHousesPending,
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
