namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx.Media;
    using MediaResponse = Husa.MediaService.Api.Contracts.Response;

    public static class MediaIdxProjection
    {
        public static ItemIdxQueryResult ToVirtualTourQueryResult(this MediaResponse.VirtualTourDetail item) => new()
        {
            Uri = item.Uri,
            Title = item.Title,
        };

        public static ImageIdxQueryResult ToImageQueryResult(this MediaResponse.MediaDetail item) => new()
        {
            Uri = item.Uri,
            Title = item.Title,
            Order = item.Order,
        };
    }
}
