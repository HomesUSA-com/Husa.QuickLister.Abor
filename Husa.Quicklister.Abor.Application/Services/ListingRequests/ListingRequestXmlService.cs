namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Extensions.XML;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using ExtensionServices = Husa.Quicklister.Extensions.Application.Services.ListingRequests;

    public class ListingRequestXmlService : ExtensionServices.ListingRequestXmlService<SaleListingRequest, ISaleListingRequestRepository, XmlListingDetailResponse>
    {
        public ListingRequestXmlService(
            ISaleListingRequestMediaService mediaService,
            ISaleListingRequestRepository requestRepository,
            IUserContextProvider userContextProvider,
            ILogger<ListingRequestXmlService> logger)
            : base(mediaService, requestRepository, userContextProvider, logger)
        {
        }

        protected override CommandSingleResult<SaleListingRequest, ValidationResult> GenerateRequest(
            IListing listing,
            SaleListingRequest oldCompleteRequest,
            XmlListingDetailResponse spec,
            Guid userId,
            bool ignoreRequestByCompletionDate = false,
            bool ignoreRequestByDescription = false)
        {
            var newRequest = oldCompleteRequest.Clone();
            newRequest.UpdateXromXml(spec, ignoreRequestByCompletionDate: ignoreRequestByCompletionDate, ignoreRequestByDescription: ignoreRequestByDescription);
            newRequest.UpdateTrackValues(userId, isNewRecord: true);
            newRequest.MlsNumber = listing.MlsNumber;
            newRequest.ListDate = listing.ListDate;
            return listing.AddRequest(newRequest, userId);
        }
    }
}
