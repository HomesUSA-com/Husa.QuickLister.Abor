namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Collections.Generic;
    using Husa.MediaService.Api.Contracts.Response;

    public class MediaComparer : IEqualityComparer<MediaDetail>
    {
        public bool Equals(MediaDetail x, MediaDetail y)
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
                   x.Uri == y.Uri &&
                   x.IsPrimary == y.IsPrimary &&
                   x.Order == y.Order &&
                   x.UriSmall == y.UriSmall &&
                   x.UriMedium == y.UriMedium;
        }

        public int GetHashCode(MediaDetail media)
        {
            if (ReferenceEquals(media, null))
            {
                return 0;
            }

            int hasTitle = !string.IsNullOrEmpty(media.Title) ? media.Title.GetHashCode() : 0;
            int hasDescription = !string.IsNullOrEmpty(media.Description) ? media.Description.GetHashCode() : 0;
            int hasUri = media.Uri.GetHashCode();
            int hasIsPrimary = media.IsPrimary.GetHashCode();
            int hasUriSmall = media.UriSmall.GetHashCode();
            int hasUriMedium = media.UriMedium.GetHashCode();

            return hasTitle ^
                   hasDescription ^
                   hasUri ^
                   hasIsPrimary ^
                   hasUriSmall ^
                   hasUriMedium;
        }
    }
}
