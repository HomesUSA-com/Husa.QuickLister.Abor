namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingDto
    {
        public virtual string AltPhoneCommunity { get; set; }
        public virtual string AgentListApptPhone { get; set; }
        public virtual Showing? Showing { get; set; }
        public virtual string RealtorContactEmail { get; set; }
        public virtual string Directions { get; set; }
    }
}
