namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingRequest
    {
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
