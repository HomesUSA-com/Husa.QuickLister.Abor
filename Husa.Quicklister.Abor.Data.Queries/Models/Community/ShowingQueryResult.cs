namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ShowingQueryResult
    {
        public virtual string AltPhoneCommunity { get; set; }
        public virtual string AgentListApptPhone { get; set; }
        public virtual Showing? Showing { get; set; }
        public virtual string RealtorContactEmail { get; set; }
        public virtual string Directions { get; set; }
    }
}
