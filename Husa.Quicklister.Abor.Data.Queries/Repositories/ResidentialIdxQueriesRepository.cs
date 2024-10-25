namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Client;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Queries.Projections.IdxProjection;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.EntityFrameworkCore;
    using XmlRequest = Husa.Xml.Api.Contracts.Request;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public class ResidentialIdxQueriesRepository : IResidentialIdxQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly IXmlClient xmlClient;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IMediaServiceClient mediaClient;

        public ResidentialIdxQueriesRepository(
            ApplicationQueriesDbContext context,
            IXmlClient xmlClient,
            IMediaServiceClient mediaClient,
            IServiceSubscriptionClient serviceSubscriptionClient)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.mediaClient = mediaClient ?? throw new ArgumentNullException(nameof(mediaClient));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
        }

        public async Task<IEnumerable<ResidentialIdxQueryResult>> FindByBuilderName(string builderName)
        {
            var companyId = await this.GetCompanyId(builderName);
            var query = this.context.ListingSale
                .FilterNotDeleted()
                .FilterByCompany(companyId);

            var xmlIds = query.Where(x => x.XmlListingId.HasValue).Select(x => x.XmlListingId.Value).ToList();
            var xmlListings = await this.GetXmlListings(xmlIds);

            var residentials = await query.Select(ResidentialIdxProjection.ToResidentialIdxQueryResult).ToListAsync();
            await this.FillMedia(residentials);

            return residentials
                .GroupJoin(xmlListings, residential => residential.XmlId, xmlListing => xmlListing.Id, (residential, xmlListing) => new { residential, xmlListing })
                .SelectMany(residentialWithXml => residentialWithXml.xmlListing.DefaultIfEmpty(), (residentialWithXml, xmlListing) =>
                {
                    residentialWithXml.residential.SpecNumber = xmlListing?.Number;
                    return residentialWithXml.residential;
                });
        }

        private async Task FillMedia(IEnumerable<ResidentialIdxQueryResult> listings)
        {
            foreach (var item in listings)
            {
                var media = await this.mediaClient.GetResources(item.Id, MediaService.Domain.Enums.MediaType.Residential);
                item.Media = new()
                {
                    VirtualTours = media.VirtualTour.Select(x => x.ToVirtualTourQueryResult()),
                    Images = media.Media.Select(x => x.ToImageQueryResult()),
                };
            }
        }

        private async Task<IEnumerable<XmlResponse.XmlListingResponse>> GetXmlListings(IEnumerable<Guid> xmlIds)
        {
            var xmlListings = new List<XmlResponse.XmlListingResponse>();
            var takeValue = 50;
            while (xmlIds.Any())
            {
                var ids = xmlIds.Take(takeValue);
                var listings = await this.xmlClient.Listing.GetAsync(new XmlRequest.ListingRequestFilter()
                {
                    ListingsIds = ids,
                });
                xmlListings.AddRange(listings.Data);
                xmlIds = xmlIds.Skip(takeValue).ToList();
            }

            return xmlListings;
        }

        private async Task<Guid> GetCompanyId(string builderName)
        {
            var companies = await this.serviceSubscriptionClient.Company.GetAsync(new()
            {
                CompanyName = builderName,
                MarketCode = MarketCode.Austin,
            });

            if (companies == null || !companies.Data.Any())
            {
                throw new DomainException($"Company with name {builderName} was not found");
            }

            if (companies.Data.Count() > 1)
            {
                throw new DomainException($"More than one company was found with name {builderName}");
            }

            return companies.Data.Single().Id;
        }
    }
}
