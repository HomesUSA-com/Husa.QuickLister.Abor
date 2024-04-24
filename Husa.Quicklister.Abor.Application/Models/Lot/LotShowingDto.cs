namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotShowingDto
    {
        public ICollection<ShowingRequirements> ShowingRequirements { get; set; }
    }
}
