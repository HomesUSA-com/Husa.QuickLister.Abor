namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotShowing
    {
        ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
