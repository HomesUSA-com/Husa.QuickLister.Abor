namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingQueryResult
    {
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
