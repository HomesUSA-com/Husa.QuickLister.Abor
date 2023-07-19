namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityShowingResponse
    {
        public virtual string AltPhoneCommunity { get; set; }
        public virtual string AgentListApptPhone { get; set; }
        public virtual Showing? Showing { get; set; }
        public virtual string RealtorContactEmail { get; set; }
        public virtual string Directions { get; set; }
    }
}
