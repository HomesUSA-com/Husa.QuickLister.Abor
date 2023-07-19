namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Collections.Generic;
    using Husa.MediaService.Api.Contracts.Response;

    public class VirtualTourComparer : IEqualityComparer<VirtualTourDetail>
    {
        public bool Equals(VirtualTourDetail x, VirtualTourDetail y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Title == y.Title &&
                   x.Description == y.Description &&
                   x.Uri == y.Uri;
        }

        public int GetHashCode(VirtualTourDetail media)
        {
            if (ReferenceEquals(media, null))
            {
                return 0;
            }

            int hasTitle = !string.IsNullOrEmpty(media.Title) ? media.Title.GetHashCode() : 0;
            int hasDescription = !string.IsNullOrEmpty(media.Description) ? media.Description.GetHashCode() : 0;
            int hasUri = media.Uri.GetHashCode();

            return hasTitle ^
                   hasDescription ^
                   hasUri;
        }
    }
}
