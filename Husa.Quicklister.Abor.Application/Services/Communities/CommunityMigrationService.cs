namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;
    using PhotoRequest = Husa.PhotoService.Api.Contracts.Request;

    public class CommunityMigrationService : ExtensionsServices.CommunityMigrationService<CommunitySale, ICommunitySaleRepository, ICommunityPhotoService>, ICommunityMigrationService
    {
        private readonly IMapper mapper;

        public CommunityMigrationService(
            ICommunitySaleRepository communityRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ICommunityPhotoService photoService,
            ILogger<CommunityMigrationService> logger,
            IMapper mapper)
            : base(communityRepository, migrationClient, serviceSubscriptionClient, photoService, logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override void UpdateCommunity(CommunitySale community, CommunityResponse communityMigration)
        {
            var communityInfo = new CommunityValueObject
            {
                PropertyInfo = this.mapper.Map<Property>(communityMigration.PropertyInfo),
                ProfileInfo = this.mapper.Map<ProfileInfo>(communityMigration.ProfileInfo),
                SalesOfficeInfo = this.mapper.Map<CommunitySaleOffice>(communityMigration.SaleOffice),
                EmailLeadInfo = this.mapper.Map<Husa.Quicklister.Abor.Domain.Entities.Community.EmailLead>(communityMigration.EmailLeads),
                UtilitiesInfo = this.mapper.Map<Utilities>(communityMigration.UtilitiesInfo),
                FinancialInfo = this.mapper.Map<CommunityFinancialInfo>(communityMigration.FinancialInfo),
                SchoolsInfo = this.mapper.Map<SchoolsInfo>(communityMigration.SchoolsInfo),
                ShowingInfo = this.mapper.Map<CommunityShowingInfo>(communityMigration.ShowingInfo),
            };
            var openHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communityMigration.OpenHouses);

            communityInfo.ProfileInfo.OwnerName = community.ProfileInfo.OwnerName;
            communityInfo.ShowingInfo.OwnerName = community.ProfileInfo.OwnerName;
            community.Update(communityInfo, openHouses);
        }

        protected override CommunitySale CreateCommunity(CompanyDetail company, CommunityResponse communityMigration)
        {
            var community = this.mapper.Map<CommunitySale>(communityMigration);
            var openHouses = this.mapper.Map<IEnumerable<CommunityOpenHouse>>(communityMigration.OpenHouses);
            community.UpdateOpenHouse(openHouses);
            community.UpdateCompanyInfo(company.Id, company.Name);
            community.Showing.OwnerName = company.Name;
            return community;
        }

        protected override PhotoRequest.Property GetPhotoRequestProperty(CommunitySale community)
        {
            return new PhotoRequest.Property()
            {
                Id = community.Id,
                Type = PhotoService.Domain.Enums.PropertyType.Community,
                Subdivision = community.Property.Subdivision,
                Zip = community.Property.ZipCode,
                City = community.Property.City.ToString(),
                ReadableCity = community.Property.City.GetEnumDescription(),
            };
        }
    }
}
