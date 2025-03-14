namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Extensions.JsonImport;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionServices = Husa.Quicklister.Extensions.Application.Services.JsonImport;

    public class ListingRequestJsonImportService : ExtensionServices.ListingRequestJsonImportService<SaleListingRequest, ISaleListingRequestRepository>
    {
        public ListingRequestJsonImportService(
            ISaleListingRequestMediaService mediaService,
            ISaleListingRequestRepository requestRepository,
            IUserContextProvider userContextProvider,
            IRequestErrorRepository requestErrorRepository,
            ILogger<ListingRequestJsonImportService> logger)
            : base(mediaService, requestRepository, userContextProvider, requestErrorRepository, logger)
        {
        }

        protected override CommandSingleResult<SaleListingRequest, ValidationResult> GenerateRequest(IListing listing, SaleListingRequest oldCompleteRequest, SpecDetailResponse spec, Guid userId)
        {
            var newRequest = oldCompleteRequest.Clone();
            newRequest.Import(spec);
            newRequest.UpdateTrackValues(userId, isNewRecord: true);
            newRequest.MlsNumber = listing.MlsNumber;
            newRequest.ListDate = listing.ListDate;

            return listing.AddRequest(newRequest, userId);
        }
    }
}
