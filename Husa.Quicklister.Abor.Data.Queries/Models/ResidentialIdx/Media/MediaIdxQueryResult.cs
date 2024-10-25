namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx.Media
{
    using System.Collections.Generic;

    public class MediaIdxQueryResult
    {
        public IEnumerable<ImageIdxQueryResult> Images { get; set; }
        public IEnumerable<ItemIdxQueryResult> VirtualTours { get; set; }
    }
}
