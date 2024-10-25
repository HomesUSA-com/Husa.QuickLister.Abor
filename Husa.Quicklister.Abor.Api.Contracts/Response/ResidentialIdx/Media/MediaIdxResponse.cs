namespace Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx.Media
{
    using System.Collections.Generic;

    public class MediaIdxResponse
    {
        public IEnumerable<ImageIdxResponse> Images { get; set; }
        public IEnumerable<ItemIdxResponse> VirtualTours { get; set; }
    }
}
