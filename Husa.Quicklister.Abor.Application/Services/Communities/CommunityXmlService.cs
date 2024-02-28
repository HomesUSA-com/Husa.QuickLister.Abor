namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using CommunityExtensions = Husa.Quicklister.Extensions.Application.Services.Communities;

    public class CommunityXmlService : CommunityExtensions.CommunityXmlService<CommunitySale, ICommunitySaleRepository>, ICommunityXmlService
    {
        public CommunityXmlService(
            IXmlClient xmlClient,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            ILogger<CommunityXmlService> logger)
            : base(xmlClient, communityRepository, userContextProvider, logger)
        {
        }

        public override async Task ImportEntity(Guid companyId, string companyName, Guid entityId)
        {
            this.Logger.LogInformation("Importing xml subdivision with id {subdivisionId} to company with id {companyId}", entityId, companyId);
            var subdivision = await this.XmlClient.Subdivision.GetByIdAsync(entityId);
            var community = await this.CreateCommunity(subdivision, companyId, subdivision.Name, companyName, subdivision.City, subdivision.County);
            this.Logger.LogDebug("Importing xml subdivision {subdivisionId} with the values: {@subdivision}", entityId, subdivision);
            community.ProcessXmlData(subdivision, companyName);
            await this.CommunitySaleRepository.SaveChangesAsync();
            await this.XmlClient.Subdivision.ProcessSubdivision(entityId, community.Id);
        }

        protected override async Task<CommunitySale> CreateCommunity(SubdivisionResponse subdivision, Guid companyId, string subdivisionName, string companyName, string city, string county)
        {
            if (subdivision.CommunityProfileId.HasValue && subdivision.CommunityProfileId != Guid.Empty)
            {
                return await this.CommunitySaleRepository.GetById(subdivision.CommunityProfileId.Value, filterByCompany: true) ??
                    throw new NotFoundException<CommunitySale>(subdivision.CommunityProfileId);
            }

            var community = new CommunitySale(
                companyId,
                subdivision.Name,
                companyName);

            this.CommunitySaleRepository.Attach(community);
            return community;
        }
    }
}
