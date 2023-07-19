namespace Husa.Quicklister.Abor.Api.Contracts.Response.PhotoRequest
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PhotoRequestPropertyResponse
    {
        public Guid Id { get; set; }

        public string StreetNum { get; set; }

        public string StreetName { get; set; }

        public string StreetType { get; set; }

        public string StreetDirection { get; set; }

        public string UnitNumber { get; set; }

        public Cities City { get; set; }

        public string Zip { get; set; }

        public States? State { get; set; }

        public string Subdivision { get; set; }

        public int CompletedPhotoRequestCount { get; set; }

        public string PlanName { get; set; }

        public string MlsNumber { get; set; }
    }
}
