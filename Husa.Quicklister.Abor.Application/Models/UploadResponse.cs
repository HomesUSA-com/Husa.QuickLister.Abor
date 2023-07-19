namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using Husa.Quicklister.Abor.Application.Models.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class UploadResponse
    {
        public ReverseProspectListingDto Listing { get; set; }

        public UploadResult Result { get; set; }

        public Exception Exception { get; set; }

        public string Data { get; set; }

        public static UploadResponse Success(ReverseProspectListingDto listing, string data) => new()
        {
            Listing = listing,
            Result = UploadResult.Success,
            Data = data,
        };

        public static UploadResponse FailedToLogin(ReverseProspectListingDto listing, string username) => new()
        {
            Listing = listing,
            Result = UploadResult.Failure,
            Data = $"failed to login with user: {username}.",
        };

        public static UploadResponse Failed(ReverseProspectListingDto listing, Exception exception, string data = null) => new()
        {
            Exception = exception,
            Listing = listing,
            Result = UploadResult.Failure,
            Data = data ?? "Error occurred while retrieving data. Please contact service provider.",
        };
    }
}
