namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;

    public class CommunityXmlService : ICommunityXmlService
    {
        private readonly ILogger<CommunityXmlService> logger;
        private readonly IXmlClient xmlClient;
        private readonly ICommunitySaleRepository communityRepository;

        public CommunityXmlService(
            IXmlClient xmlClient,
            ICommunitySaleRepository communityRepository,
            ILogger<CommunityXmlService> logger)
        {
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.communityRepository = communityRepository ?? throw new ArgumentNullException(nameof(communityRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ImportEntity(Guid companyId, string companyName, Guid entityId)
        {
            this.logger.LogInformation("Importing xml subdivision with id {subdivisionId} to company with id {companyId}", entityId, companyId);
            var subdivision = await this.xmlClient.Subdivision.GetByIdAsync(entityId);
            var community = await GetCommunity(companyId, subdivision, companyName);
            this.logger.LogDebug("Importing xml subdivision {subdivisionId} with the values: {@subdivision}", entityId, subdivision);
            community.ProcessXmlData(subdivision, companyName);
            await this.communityRepository.SaveChangesAsync();
            await this.xmlClient.Subdivision.ProcessSubdivision(entityId, community.Id);

            async Task<CommunitySale> GetCommunity(Guid companyId, SubdivisionResponse subdivision, string companyName)
            {
                if (subdivision.CommunityProfileId.HasValue && subdivision.CommunityProfileId != Guid.Empty)
                {
                    return await this.communityRepository.GetById(subdivision.CommunityProfileId.Value, filterByCompany: true) ??
                        throw new NotFoundException<CommunitySale>(subdivision.CommunityProfileId);
                }

                var community = new CommunitySale(companyId, subdivision.Name, companyName);
                this.communityRepository.Attach(community);
                return community;
            }
        }

        public async Task ApproveCommunity(Guid communityId)
        {
            var community = await this.communityRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            this.logger.LogInformation("Starting approve sale community with id {communityId}", communityId);
            community.Approve();
            await this.communityRepository.SaveChangesAsync();
            await this.xmlClient.Subdivision.ApproveSubdivisionByLinkedId(communityId);
        }
    }
}
