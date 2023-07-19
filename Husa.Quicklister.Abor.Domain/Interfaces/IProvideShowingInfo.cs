namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideShowingInfo
    {
        string AltPhoneCommunity { get; set; }
        string AgentListApptPhone { get; set; }
        Showing? Showing { get; set; }
        string RealtorContactEmail { get; set; }
        string Directions { get; set; }
    }
}
