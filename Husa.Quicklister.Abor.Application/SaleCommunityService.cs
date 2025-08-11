namespace Husa.Quicklister.Abor.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.ShowingTime.Models;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Extensions;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Communities;

    public class SaleCommunityService : ExtensionsServices.SaleCommunityService<
        CommunitySale,
        ICommunitySaleRepository,
        CommunityEmployee>, ISaleCommunityService
    {
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly ICommunityHistoryService communityHistoryService;
        private readonly IMapper mapper;

        public SaleCommunityService(
            ICommunitySaleRepository communitySaleRepository,
            ICommunityHistoryService communityHistoryService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            ICommunityDeletionService communityDeletionService,
            IMapper mapper,
            ILogger<SaleCommunityService> logger)
            : base(communitySaleRepository, userContextProvider, communityDeletionService, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.communityHistoryService = communityHistoryService ?? throw new ArgumentNullException(nameof(communityHistoryService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CommandSingleResult<Guid, string>> CreateAsync(CommunitySaleCreateDto communitySaleDto)
        {
            this.Logger.LogDebug("Starting Create community sale service {@communityDto}.", communitySaleDto);
            var communityFound = await this.CommunitySaleRepository.GetCommunity(communitySaleDto.Name, communitySaleDto.CompanyId);
            if (communityFound is not null)
            {
                throw new DomainException($"Community '{communityFound.ProfileInfo.Name}' for the company '{communityFound.ProfileInfo.OwnerName}' already exists!");
            }

            var user = this.UserContextProvider.GetCurrentUser();
            var company = await this.serviceSubscriptionClient.Company.GetCompany(communitySaleDto.CompanyId);
            var community = new CommunitySale(company.Id, communitySaleDto.Name, company.Name);
            community.UpdateAddressInfo(communitySaleDto.City, communitySaleDto.County);
            community.UpdateCompanyEmailLeads(company.EmailLeads);
            if (user is not null && user.EmployeeRole == RoleEmployee.SalesEmployee)
            {
                community.AddCommunityEmployee(user.Id);
            }

            this.CommunitySaleRepository.Attach(community);

            if (await this.serviceSubscriptionClient.HasService(community.CompanyId, ServiceCode.ShowingTime) && company.ShowingTime != null)
            {
                community.UpdateShowingTime(true, company.ShowingTime);
            }

            await this.CommunitySaleRepository.SaveChangesAsync();
            await this.communityHistoryService.CreateRecordAsync(community.Id, isSubmitted: false);
            return CommandSingleResult<Guid, string>.Success(community.Id);
        }

        public async Task UpdateCommunity(Guid communityId, CommunitySaleDto communitySaleDto, bool isSubmitted = false)
        {
            var community = await this.CommunitySaleRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            this.Logger.LogInformation("Starting update sale community with id {communityId}", communityId);
            var user = this.UserContextProvider.GetCurrentUser();
            community.CanUpdate(user);
            var communityFound = await this.CommunitySaleRepository.GetCommunity(communitySaleDto.Profile.Name, communitySaleDto.CompanyId);

            if (communityFound is not null && communityFound.Id != community.Id)
            {
                throw new DomainException($"Community '{communityFound.ProfileInfo.Name}' for the company '{communityFound.ProfileInfo.OwnerName}' already exists!");
            }

            var communityInfo = new CommunityValueObject
            {
                PropertyInfo = this.mapper.Map<Property>(communitySaleDto.Property),
                ProfileInfo = this.mapper.Map<ProfileInfo>(communitySaleDto.Profile),
                SalesOfficeInfo = this.mapper.Map<CommunitySaleOffice>(communitySaleDto.Profile.SalesOffice),
                EmailLeadInfo = this.mapper.Map<EmailLead>(communitySaleDto.Profile.EmailLead),
                UtilitiesInfo = this.mapper.Map<Utilities>(communitySaleDto.Utilities),
                FinancialInfo = this.mapper.Map<CommunityFinancialInfo>(communitySaleDto.FinancialSchools),
                SchoolsInfo = this.mapper.Map<SchoolsInfo>(communitySaleDto.FinancialSchools.Schools),
                ShowingInfo = this.mapper.Map<CommunityShowingInfo>(communitySaleDto.Showing),
                ShowingTime = this.mapper.Map<ShowingTime>(communitySaleDto.ShowingTime),
                UseShowingTime = communitySaleDto.UseShowingTime,
            };
            var communityOpenHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communitySaleDto.OpenHouses);

            community.Update(communityInfo, communityOpenHouses);

            await this.CommunitySaleRepository.SaveChangesAsync();
            await this.communityHistoryService.CreateRecordAsync(communityId, isSubmitted);
        }

        public async Task<bool> UpdateCommunityOpenHouses(Guid communityId, CommunitySaleDto communityDto, bool isSubmitted = false)
        {
            var community = await this.CommunitySaleRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            this.Logger.LogInformation("Starting update sale community open-houses with id {communityId}", communityId);
            var user = this.UserContextProvider.GetCurrentUser();
            community.CanUpdate(user);
            var communityFound = await this.CommunitySaleRepository.GetCommunity(communityDto.Profile.Name, communityDto.CompanyId);

            if (communityFound is not null && communityFound.Id != community.Id)
            {
                throw new DomainException($"Community '{communityFound.ProfileInfo.Name}' for the company '{communityFound.ProfileInfo.OwnerName}' already exists!");
            }

            community.Showing.EnableOpenHouses = communityDto.Showing.EnableOpenHouses;
            community.UpdateOpenHouses(this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communityDto.OpenHouses));

            await this.CommunitySaleRepository.SaveChangesAsync();
            await this.communityHistoryService.CreateRecordAsync(communityId, isSubmitted);

            return community.HasOpenHouseChangesToSubmit;
        }

        public async Task UpdateListingsAsync(Guid communityId)
        {
            var community = await this.CommunitySaleRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            this.Logger.LogInformation("Starting update listing from community with id {communityId}", communityId);
            community.UpdateListingsFromCommunityAsync<SaleListing, CommunitySale>();
            await this.CommunitySaleRepository.SaveChangesAsync();
        }
    }
}
