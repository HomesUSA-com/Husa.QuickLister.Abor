namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotShowing : IProvideCommonShowingInfo
    {
        ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        ICollection<ShowingContactType> ShowingContactType { get; set; }
        string ApptPhone { get; set; }
        string ShowingServicePhone { get; set; }
        string PublicRemarks { get; set; }
    }
}
