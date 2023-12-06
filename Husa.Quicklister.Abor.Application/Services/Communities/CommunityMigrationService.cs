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
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;
    using PhotoRequest = Husa.PhotoService.Api.Contracts.Request;

    public class CommunityMigrationService : ExtensionsServices.CommunityMigrationService<CommunitySale, ICommunitySaleRepository, ICommunityPhotoService>
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
