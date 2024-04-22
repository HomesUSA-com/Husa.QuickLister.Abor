namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingResponse
    {
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
