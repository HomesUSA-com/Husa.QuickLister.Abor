namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.MediaService.Domain.Enums;
    using Husa.PhotoService.Domain.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Notes;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
    using Moq;
    using DomainEntities = Husa.Quicklister.Abor.Domain.Entities;
    using HusaNoteType = Husa.Notes.Domain.Enums.NoteType;
    using MediaRequest = Husa.MediaService.Api.Contracts.Request;
    using RequestNote = Husa.Notes.Api.Contracts.Request;
    using RequestPhoto = Husa.PhotoService.Api.Contracts.Request;
    using ResponseNote = Husa.Notes.Api.Contracts.Response;
    using ResponsePhoto = Husa.PhotoService.Api.Contracts.Response;
    using States = Husa.Extensions.Common.Enums.States;

    public static class TestModelProvider
    {
        public static ListingSaleQueryResult GetListingSaleQueryResult(Guid? id) => new()
        {
            Id = id ?? Guid.NewGuid(),
            City = Faker.Enum.Random<Cities>(),
            StreetName = Faker.Address.StreetName(),
            StreetNum = "1234",
            MlsStatus = MarketStatuses.Active,
            ListDate = DateTime.UtcNow,
            ListPrice = 150000,
            MlsNumber = "123456",
            ZipCode = Faker.Address.ZipCode()[..5],
            Subdivision = Faker.Address.City(),
            County = Faker.Enum.Random<Counties>(),
            MarketModifiedOn = DateTime.UtcNow,
            SysModifiedOn = DateTime.UtcNow,
        };

        public static ListingSaleResponse GetListingSaleResponse(Guid? id) => new()
        {
            Id = id ?? Guid.NewGuid(),
            City = Faker.Enum.Random<Cities>(),
            StreetName = Faker.Address.StreetName(),
            StreetNum = "1234",
            MlsStatus = MarketStatuses.Active,
            ListDate = DateTime.UtcNow,
            ListPrice = 150000,
            MlsNumber = "123456",
            ZipCode = Faker.Address.ZipCode()[..5],
            Subdivision = Faker.Address.City(),
            County = Counties.Angelina,
            MarketModifiedOn = DateTime.UtcNow,
            SysModifiedOn = DateTime.UtcNow,
        };

        public static ListingSaleQueryDetailResult GetListingSaleQueryDetail(Guid? id, Guid? companyId = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            MlsStatus = MarketStatuses.Active,
            ListDate = DateTime.UtcNow,
            ListPrice = 150000,
            MlsNumber = "123456",
            MarketModifiedOn = DateTime.UtcNow,
            SysModifiedOn = DateTime.UtcNow,
            SaleProperty = new SalePropertyDetailQueryResult()
            {
                SalePropertyInfo = new SalePropertyQueryResult()
                {
                    CompanyId = companyId ?? Guid.NewGuid(),
                },
            },
        };

        public static ListingSaleRequest GetListingSaleRequest(Guid? companyId = null, Guid? communityId = null, Guid? planId = null, Guid? listingIdToImport = null) => new()
        {
            MlsStatus = MarketStatuses.Active,
            City = Faker.Enum.Random<Cities>(),
            CompanyId = companyId ?? Guid.NewGuid(),
            CommunityId = communityId,
            ListingIdToImport = listingIdToImport,
            PlanId = planId ?? Guid.NewGuid(),
            ConstructionCompletionDate = DateTime.UtcNow,
            County = Counties.Angelina,
            State = States.Texas,
            StreetName = Faker.Address.StreetName(),
            StreetNumber = "123",
            ZipCode = Faker.Address.ZipCode()[..5],
        };

        public static CreateCommunityRequest GetCommunitySaleRequest() => new()
        {
            CompanyId = Guid.NewGuid(),
            City = Faker.Enum.Random<Cities>(),
            Name = Faker.Name.FullName(),
            County = Counties.Angelina,
            OwnerName = Faker.Address.StreetName(),
        };

        public static Mock<CommunitySale> GetCommunitySaleEntityMock(Guid? communityId, Guid? companyId)
        {
            var communitySale = new Mock<CommunitySale>();
            communitySale.SetupAllProperties();
            communitySale.SetupGet(c => c.Id).Returns(communityId ?? Guid.NewGuid());
            communitySale.SetupGet(c => c.CompanyId).Returns(companyId ?? Guid.NewGuid());

            return communitySale;
        }

        public static CommunitySale GetCommunitySaleEntity(Guid? communityId, Guid? companyId = null, bool hasActiveListing = false)
        {
            var saleProperty = GetFullSalePropertyWithStaticValues(id: Guid.NewGuid(), communityId: communityId, companyId: companyId);
            if (hasActiveListing)
            {
                var listingSale = GetListingSaleEntity(Guid.NewGuid(), true);
                listingSale.MlsStatus = MarketStatuses.Active;
                listingSale.MlsNumber = "123456";
                saleProperty.SaleListings = new List<DomainEntities.Listing.SaleListing>() { listingSale };
            }

            var openHouses = new List<CommunityOpenHouse>() { new CommunityOpenHouse(Guid.NewGuid(), Faker.Enum.Random<OpenHouseType>(), DateTime.UtcNow.TimeOfDay, DateTime.UtcNow.TimeOfDay, false, false) };

            var communitySale = new CommunitySale(
                companyId: companyId ?? Guid.NewGuid(),
                name: "Community",
                Faker.Company.Name())
            {
                SaleProperties = new List<SaleProperty>() { saleProperty, GetFullSalePropertyWithStaticValues() },
                OpenHouses = openHouses,
                Id = communityId ?? Guid.NewGuid(),
                Property = new()
                {
                    City = Faker.Enum.Random<Cities>(),
                    County = Faker.Enum.Random<Counties>(),
                },
            };

            return communitySale;
        }

        public static CommunitySaleCreateDto GetCommunitySaleCreateDto(Guid? companyId) => new()
        {
            City = Faker.Enum.Random<Cities>(),
            CompanyId = companyId ?? Guid.NewGuid(),
            County = Faker.Enum.Random<Counties>(),
            Name = Faker.Address.StreetName(),
            OwnerName = "123",
        };

        public static CommunitySaleDto GetCommunityUpdateDto() => new()
        {
            CompanyId = Guid.NewGuid(),
            FinancialSchools = new(),
            Hoas = new List<CommunityHoaDto>() { new() },
            Showing = new(),
            OpenHouses = new List<OpenHouseDto> { new() },
            Profile = new(),
            Property = new(),
            Utilities = new(),
        };

        public static Mock<DomainEntities.Listing.SaleListing> GetListingSaleEntityMock(Guid? listingId = null, bool createStub = false, Guid? companyId = null, Guid? communityId = null, bool generateRequest = false)
        {
            var listingSale = new Mock<DomainEntities.Listing.SaleListing>();
            if (createStub)
            {
                listingSale.SetupAllProperties();
            }

            var listingCompanyId = companyId ?? Guid.NewGuid();

            var saleProperty = GetFullSalePropertyWithStaticValues(
                id: Guid.NewGuid(),
                communityId: communityId ?? Guid.NewGuid(),
                companyId: listingCompanyId,
                planId: Guid.NewGuid(),
                addSaleListings: false);
            saleProperty.Plan = new Plan(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord());

            listingSale.SetupGet(c => c.Id).Returns(listingId ?? Guid.NewGuid());
            listingSale.SetupGet(c => c.SaleProperty).Returns(saleProperty);
            listingSale.SetupGet(c => c.StatusFieldsInfo).Returns(new ListingSaleStatusFieldsInfo());
            listingSale.SetupGet(c => c.CompanyId).Returns(listingCompanyId);
            listingSale.SetupGet(c => c.IsInMls).Returns(true);
            listingSale.Setup(x => x.MlsNumber).Returns("12345");
            saleProperty.SaleListings = new[] { listingSale.Object };

            if (!generateRequest)
            {
                listingSale.Setup(c => c.GenerateRequest(It.IsAny<Guid>())).CallBase();
            }
            else
            {
                var saleListingRequest = new Mock<SaleListingRequest>();

                saleListingRequest.SetupGet(sl => sl.Id).Returns(listingSale.Object.Id).Verifiable();

                listingSale.Setup(c => c.GenerateRequest(It.IsAny<Guid>())).Returns(CommandSingleResult<SaleListingRequest, ValidationResult>.Success(saleListingRequest.Object))
                    .Verifiable();
            }

            return listingSale;
        }

        public static DomainEntities.Listing.SaleListing GetListingSaleEntity(Guid? listingId = null, bool createStub = false, Guid? companyId = null, Guid? communityId = null, bool generateRequest = false)
        {
            var listingSale = GetListingSaleEntityMock(listingId, createStub, companyId, communityId, generateRequest);
            return listingSale.Object;
        }

        public static PropertyInfo GetPropertyInfo() => new()
        {
            ConstructionCompletionDate = DateTime.UtcNow,
            LotDescription = GetEnumCollectionRandom<LotDescription>(),
        };

        public static ListingSaleDto GetListingSaleDto(Guid? companyId, Guid? communityId = null, Guid? planId = null) => new()
        {
            MlsStatus = MarketStatuses.Active,
            City = Faker.Enum.Random<Cities>(),
            CommunityId = communityId ?? Guid.NewGuid(),
            PlanId = planId ?? Guid.NewGuid(),
            ConstructionCompletionDate = DateTime.UtcNow,
            County = Faker.Enum.Random<Counties>(),
            State = States.Texas,
            StreetName = Faker.Address.StreetName(),
            StreetNumber = "123",
            ZipCode = Faker.Address.ZipCode()[..5],
            CompanyId = companyId ?? Guid.NewGuid(),
        };

        public static SaleListingDto GetSaleListingDto() => new()
        {
            MlsStatus = MarketStatuses.Active,
            ExpirationDate = DateTime.UtcNow,
            SaleProperty = new SalePropertyDetailDto
            {
                SpacesDimensionsInfo = new SpacesDimensionsDto(),
                FeaturesInfo = new(),
                FinancialInfo = new(),
                ShowingInfo = new(),
                SchoolsInfo = new(),
                PropertyInfo = new(),
                AddressInfo = new(),
                SalePropertyInfo = new(),
                Hoas = new List<HoaDto>
                {
                    new HoaDto(),
                },
                OpenHouses = new List<OpenHouseDto>
                {
                    new(),
                },
                Rooms = new List<RoomDto>
                {
                    new(),
                },
            },
            StatusFieldsInfo = new ListingSaleStatusFieldsDto
            {
                PendingDate = DateTime.UtcNow,
            },
        };

        public static Listing GetListingEntity(Guid? listingId)
        {
            var listing = new Mock<Listing>();
            listing.SetupAllProperties();

            listing.SetupGet(c => c.Id).Returns(listingId ?? Guid.NewGuid());
            return listing.Object;
        }

        public static UserContext GetCurrentUser(Guid? userId = null, Guid? companyId = null, UserRole userRole = UserRole.User)
        {
            var userContext = new Mock<UserContext>();
            userContext.SetupAllProperties();
            userContext.Object.Id = userId ?? Guid.NewGuid();
            userContext.Object.UserRole = userRole;
            userContext.Object.CompanyId = userRole == UserRole.MLSAdministrator && companyId == null ? null : companyId ?? Guid.NewGuid();
            userContext.Object.IsMLSAdministrator = userRole == UserRole.MLSAdministrator;
            return userContext.Object;
        }

        public static CommunityQueryResult GetCommunityQueryResult(Guid? communityId)
        {
            var communityQueryResult = new Mock<CommunityQueryResult>();
            communityQueryResult.SetupAllProperties();
            communityQueryResult.Object.Name = Faker.Address.UkCounty();
            communityQueryResult.Object.Market = MarketCode.Austin;
            communityQueryResult.Object.City = Faker.Enum.Random<Cities>();
            communityQueryResult.Object.ZipCode = Faker.Address.ZipCode()[..5];
            communityQueryResult.Object.Builder = Faker.Company.Name();
            communityQueryResult.Object.Id = communityId ?? Guid.NewGuid();
            communityQueryResult.Object.SysModifiedOn = DateTime.UtcNow;

            return communityQueryResult.Object;
        }

        public static CommunityQueryFilter GetCommunityQueryFilter(Guid? companyId)
        {
            var communityQueryFilter = new Mock<CommunityQueryFilter>();
            communityQueryFilter.SetupAllProperties();
            communityQueryFilter.Object.Skip = 0;
            communityQueryFilter.Object.Take = 50;
            return communityQueryFilter.Object;
        }

        public static CommunityRequestFilter GetCommunityByCompanyRequestFilter(Guid? companyId)
        {
            var communityByCompanyRequestFilter = new Mock<CommunityRequestFilter>();
            communityByCompanyRequestFilter.SetupAllProperties();
            communityByCompanyRequestFilter.Object.SearchBy = string.Empty;
            communityByCompanyRequestFilter.Object.Skip = 0;
            communityByCompanyRequestFilter.Object.Take = 50;
            return communityByCompanyRequestFilter.Object;
        }

        public static CommunityDetailQueryResult GetCommunityDetailQueryResult(Guid? id, Guid? companyId = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            CompanyId = companyId ?? Guid.NewGuid(),
        };

        public static PlanQueryResult GetPlanQueryResult(Guid? planId)
        {
            var planQueryResult = new Mock<PlanQueryResult>();
            planQueryResult.SetupAllProperties();
            planQueryResult.Object.Name = Faker.Address.UkCounty();
            planQueryResult.Object.Market = MarketCode.Austin;
            planQueryResult.Object.OwnerName = Faker.Company.Name();
            planQueryResult.Object.Id = planId ?? Guid.NewGuid();
            planQueryResult.Object.SysModifiedOn = DateTime.UtcNow;

            return planQueryResult.Object;
        }

        public static PlanRequestFilter GetPlanByCompanyRequestFilter(Guid? companyId)
        {
            var planByCompanyRequestFilter = new Mock<PlanRequestFilter>();
            planByCompanyRequestFilter.SetupAllProperties();
            planByCompanyRequestFilter.Object.SearchBy = string.Empty;
            planByCompanyRequestFilter.Object.Skip = 0;
            planByCompanyRequestFilter.Object.Take = 50;
            return planByCompanyRequestFilter.Object;
        }

        public static PlanDetailQueryResult GetPlanDetailQueryResult(Guid? id, Guid? companyId = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Plan Test",
            OwnerName = Faker.Company.Name(),
            CompanyId = companyId ?? Guid.NewGuid(),
        };

        public static SaleListingRequest GetListingSaleRequestObject(Guid? saleRequestId)
        {
            var listingSaleRequest = new Mock<SaleListingRequest>();
            listingSaleRequest.SetupAllProperties();

            listingSaleRequest.SetupGet(c => c.Id).Returns(saleRequestId ?? Guid.NewGuid());
            return listingSaleRequest.Object;
        }

        public static ListingSaleRequestDto GetListingSaleRequestCommand(ListingRequestValueObject listingRequestValue, SalePropertyDetailDto saleProperty)
        => new()
        {
            ExpirationDate = listingRequestValue.ExpirationDate,
            ListPrice = listingRequestValue.ListPrice,
            ListDate = listingRequestValue.ListDate,
            MlsNumber = listingRequestValue.MlsNumber,
            MlsStatus = listingRequestValue.MlsStatus,
            SaleProperty = saleProperty,
        };

        public static ListingRequestValueObject GetListingRequestValue()
        {
            var listingRequestValue = new Mock<ListingRequestValueObject>();
            listingRequestValue.SetupAllProperties();
            return listingRequestValue.Object;
        }

        public static SalePropertyDetailDto GetSalePropertyDetailDto()
        {
            var salePropertyDto = new Mock<SalePropertyDetailDto>();
            salePropertyDto.SetupAllProperties();
            return salePropertyDto.Object;
        }

        public static SaleListingRequest GetListingSaleRequestEntity(
            Guid? id,
            IEnumerable<ListingSaleRoom> rooms = null,
            IEnumerable<SaleListingHoa> hoas = null)
        {
            var roomsRecord = rooms != null ? rooms
                    .Select(rooms => RoomRecord.CreateRoom(rooms))
                    .ToList() : new List<RoomRecord>();

            var hoaRecord = hoas != null ? hoas
                    .Select(hoas => HoaRecord.CreateHoa(hoas))
                    .ToList() : new List<HoaRecord>();

            var propertyRecord = new Mock<SalePropertyRecord>();
            propertyRecord.SetupAllProperties();
            propertyRecord.SetupProperty(p => p.SpacesDimensionsInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.PropertyInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.AddressInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.SchoolsInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.ShowingInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.FeaturesInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.FinancialInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.SalesOfficeInfo, initialValue: new());
            propertyRecord.SetupProperty(p => p.OpenHouses, initialValue: Array.Empty<OpenHouseRecord>());
            propertyRecord.SetupProperty(p => p.ListingSaleHoas, initialValue: hoaRecord);
            propertyRecord.SetupProperty(p => p.Rooms, initialValue: roomsRecord);

            propertyRecord
                .Setup(p => p.GetSummarySections(It.IsAny<SalePropertyRecord>()))
                .CallBase()
                .Verifiable();
            propertyRecord.Setup(p => p.UpdateHoas(It.IsAny<ICollection<SaleListingHoa>>())).CallBase();
            propertyRecord.Setup(p => p.UpdateRooms(It.IsAny<ICollection<ListingSaleRoom>>())).CallBase();

            var listingSaleRequest = new Mock<SaleListingRequest>();
            listingSaleRequest.SetupAllProperties();
            listingSaleRequest.Object.Id = id ?? Guid.NewGuid();
            listingSaleRequest.SetupGet(lr => lr.SaleProperty).Returns(propertyRecord.Object);

            var statusFieldsRecord = new Mock<StatusFieldsRecord>();
            statusFieldsRecord
                .Setup(p => p.GetSummary(It.Is<StatusFieldsRecord>(p => p == null), It.IsAny<MarketStatuses>()))
                .Returns((SummarySection)null)
                .Verifiable();
            listingSaleRequest.SetupGet(s => s.StatusFieldsInfo).Returns(statusFieldsRecord.Object);

            return listingSaleRequest.Object;
        }

        public static UpdatePlanRequest GetPlanUpdateRequest()
        {
            var planRequest = new Mock<UpdatePlanRequest>();
            planRequest.SetupAllProperties();

            return planRequest.Object;
        }

        public static CreatePlanRequest GetPlanRequest() => new()
        {
            CompanyId = Guid.NewGuid(),
            Name = Faker.Name.FullName(),
            OwnerName = Faker.Company.Name(),
        };

        public static Plan GetPlanEntity(Guid? planId = null, bool createStub = false, Guid? companyId = null)
        {
            var plan = new Mock<Plan>();
            var basePlan = new BasePlan("test", "test");
            var company = companyId ?? Guid.NewGuid();
            var planProfileId = planId ?? Guid.NewGuid();

            if (createStub)
            {
                plan.SetupAllProperties();
            }

            plan.SetupGet(x => x.BasePlan).Returns(basePlan).Verifiable();

            var saleProperties = new List<SaleProperty>()
            {
                GetFullSalePropertyWithStaticValues(planId: planProfileId, companyId: company),
                GetSalePropertyEntityWithOpenHouses(planId: planProfileId, companyId: company),
            };

            plan.SetupGet(c => c.Id).Returns(planProfileId);
            plan.SetupGet(c => c.CompanyId).Returns(company);
            plan.SetupGet(c => c.SaleProperties).Returns(saleProperties);
            return plan.Object;
        }

        public static PlanCreateDto GetPlanCreateDto(Guid? companyId) => new()
        {
            CompanyId = companyId ?? Guid.NewGuid(),
            Name = Faker.Name.FullName(),
            OwnerName = Faker.Company.Name(),
        };

        public static UpdatePlanDto GetPlanDto()
        {
            var plan = new UpdatePlanDto
            {
                CompanyId = Guid.NewGuid(),
                Rooms = new List<RoomDto>() { new RoomDto() },
            };

            return plan;
        }

        public static CompanyDetail GetCompanyDetail(Guid? companyId = null) => new()
        {
            Id = companyId ?? Guid.NewGuid(),
            Name = Faker.Company.Name(),
        };

        public static MediaDetail GetMediaDetail(Guid? mediaId = null) => new()
        {
            Id = mediaId ?? Guid.NewGuid(),
            IsPrimary = true,
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
            Uri = new Uri(Faker.Internet.Url()),
            UriSmall = new Uri(Faker.Internet.Url()),
            UriMedium = new Uri(Faker.Internet.Url()),
            Order = 1,
        };

        public static VirtualTourDetail GetVirtualTourDetail(Guid? entityId = null) => new()
        {
            Id = Guid.NewGuid(),
            Title = Faker.Lorem.GetFirstWord(),
            EntityId = entityId ?? Guid.NewGuid(),
            Uri = new Uri(Faker.Internet.Url()),
        };

        public static VirtualTour VirtualTour(Guid? entityId = null) => new()
        {
            Id = Guid.NewGuid(),
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
            EntityId = entityId ?? Guid.NewGuid(),
            Type = MediaType.Residential,
            Uri = new Uri(Faker.Internet.Url()),
        };

        public static MediaRequest.Media MediaDetail(Guid? entityId = null) => new()
        {
            Id = Guid.NewGuid(),
            IsPrimary = true,
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
            Order = 1,
            EntityId = entityId ?? Guid.NewGuid(),
            Type = MediaType.Residential,
        };

        public static MediaFilter GetMediaFilter(Guid? ownerId) => new()
        {
            OwnerId = ownerId ?? Guid.NewGuid(),
            Type = MediaType.Residential,
        };

        public static RequestPhoto.Property GetProperty(PhotoService.Domain.Enums.PropertyType? type = null) => new()
        {
            Id = Guid.NewGuid(),
            Type = type ?? PhotoService.Domain.Enums.PropertyType.Residential,
            City = Faker.Enum.Random<Cities>().ToStringFromEnumMember(),
        };

        public static RequestPhoto.PhotoRequest GetPhotoRequest(PhotoService.Domain.Enums.PropertyType? type = null) => new()
        {
            ExteriorOptions = new List<ExteriorOptionType>(),
            Phones = new List<RequestPhoto.Phones>(),
            SecondaryEmail = Faker.Internet.Email(),
            PrimaryEmail = Faker.Internet.Email(),
            KeyLocation = Faker.Lorem.GetFirstWord(),
            SpecialNotes = Faker.Lorem.Sentence(),
            OtherInstructions = Faker.Lorem.Paragraph(),
            Directions = Faker.Address.SecondaryAddress(),
            Assistant = Faker.Name.FullName(),
            Property = GetProperty(type),
            CompanyName = Faker.Company.Name(),
            ScheduleDate = DateTime.UtcNow,
            ContactDate = DateTime.UtcNow,
            Status = PhotoRequestStatus.Pending,
            InteriorOptions = new List<InteriorOptionType>(),
            ServiceOptions = new List<ServiceOptionType>(),
            CommunityInfo = type == PhotoService.Domain.Enums.PropertyType.Community ? new() : null,
            CompanyId = Guid.NewGuid(),
        };

        public static ResponsePhoto.PhotoRequestResponse GetPhotoRequestResponse(PhotoRequestType? type = null) => new()
        {
            Type = type ?? PhotoRequestType.Residential,
            Status = PhotoRequestStatus.Pending,
            SubmitDate = DateTime.UtcNow,
            ContactDate = DateTime.UtcNow,
            ScheduleDate = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow,
            CompanyName = Faker.Company.Name(),
        };

        public static ResponsePhoto.PhotoRequestDetail GetPhotoRequestDetail() => new()
        {
            Assistant = Faker.Name.FullName(),
            Directions = Faker.Address.SecondaryAddress(),
            OtherInstructions = Faker.Lorem.Paragraph(),
            SpecialNotes = Faker.Lorem.Sentence(),
            KeyLocation = Faker.Lorem.GetFirstWord(),
            SecondaryEmail = Faker.Internet.Email(),
            PrimaryEmail = Faker.Internet.Email(),
            Type = PhotoRequestType.Residential,
            Status = PhotoRequestStatus.Pending,
            SubmitDate = DateTime.UtcNow,
            ContactDate = DateTime.UtcNow,
            ScheduleDate = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow,
            CompanyName = Faker.Company.Name(),
            InteriorOptions = new List<InteriorOptionType>(),
            ServiceOptions = new List<ServiceOptionType>(),
            ExteriorOptions = new List<ExteriorOptionType>(),
            Phones = new List<ResponsePhoto.PhotoRequestPhoneType>(),
        };

        public static NoteRequest GetNoteRequest(Guid? noteId) => new()
        {
            Id = noteId ?? Guid.NewGuid(),
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
        };

        public static RequestNote.Note GetRequestNote(Guid? noteId) => new()
        {
            Id = noteId ?? Guid.NewGuid(),
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
        };

        public static ResponseNote.Note GetResponseNote(Guid? noteId) => new()
        {
            Id = noteId ?? Guid.NewGuid(),
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
        };

        public static NoteDetailResult GetNoteDetailResult(Guid? noteId) => new()
        {
            Id = noteId ?? Guid.NewGuid(),
            Description = Faker.Lorem.Sentence(),
            Title = Faker.Lorem.GetFirstWord(),
        };

        public static RequestNote.NoteFilter GetNoteFilter(Guid? ownerId) => new()
        {
            EntityId = ownerId ?? Guid.NewGuid(),
            Type = HusaNoteType.Residential,
            MarketCode = MarketCode.Austin,
        };
        public static ResidentialMessage GetResidentialMessage() => new()
        {
            ResidentialValue = GetResidentialValueMessage(),
            Rooms = new List<RoomMessage>()
              {
                  GetRoomMessage(),
                  GetRoomMessage(),
              },
        };

        public static ResidentialValueMessage GetResidentialValueMessage() => new()
        {
            Accessibility = null,
            AccessibilityDetails = "YES",
            AdditionalParking = null,
            Bedrooms = 4,
            Block = "26",
            Contract = "ERS",
            ElementarySchool = "Holland",
            EnergyFeatures = null,
            FullBaths = 1,
            GreenCertification = null,
            GreenFeatures = null,
            HalfBaths = "2",
            HighSchool = "Holland",
            ListingAgentId = "365976",
            CoolingSystemDescription = "RAMPD",
            ExteriorFeatures = "WKSHP,STRG",
            Exterior = "WOOD",
            Fireplaces = "2",
            FireplaceDescription = "FAMRM,GASSTARTER,WDBRN",
            Flooring = "VINYL,WOOD",
            Foundation = "CEDAR",
            HomeFaces = null,
            Inclusions = "CLFNS,WSHCN,DRYCN,MCOVN,STVRN,REFRI,GSWTR",
            Improvements = "ALLEY,ASPHALT",
            MasterBedroom = "SPLIT",
            NeighborhoodAmenities = NeighborhoodAmenities.Playground.ToStringFromEnumMember(),
            Occupancy = "VACNT",
            GarageDescription = "NONE",
            PrivatePoolDescription = "NONE",
            ProposedTerms = "CONV,FHA",
            Roof = "COMP",
            PropertyType = "SFD",
            WaterSewer = "SWRSY",
            WindowCoverings = "SOME",
            County = "MEDINA",
            TaxByMultipleCounties = "NO",
            MultipleHOA = null,
            Construction = "PRE-OWNED",
            OwnerLREALREB = "NO",
            TitleCompany = "Mission Title Co.",
            Showing = "AGENT",
            SchoolDistrict = "Holland",
            TaxId = "16468",
            BuilderName = "unknown",
            SaleTerms2nd = null,
            UtilitySupplierGas = "Other",
            UtilitySupplierElectricity = "Other",
            UtilitySupplierGarbage = null,
            UtilitySupplierWater = null,
            UtilitySupplierSewer = null,
            UtilitySupplierOther = null,
            ApptPhone = "830 931 4937",
            ContigencyInfo = null,
            SoldSqft = null,
            Directions = "Centrally located in the city of Hondo, TX, 1 street south of US HWY 90 (19th St) and 1 block West of Avenue M on the CORNER  of 20th Street and Avenue N.",
            AgentPrivateRemarks = null,
            OwnerName = "M/M   Salazar",
            LegalDescription = "Lots  6 and 7, Block 26, City ",
            MapscoGrid = "3000",
            SubAgent = "0%",
            BuyerAgentComission = "3",
            PreDirections = null,
            StreetName = "1301",
            StreetNumber = null,
            MlsArea = 3000,
            ListPrice = 199000,
            CDOM = 203,
            City = "Holland",
            Type = "RE",
            CloseDate = null,
            ContractDate = null,
            DOM = 203,
            ExpirationDate = null,
            PhotoTimestamp = DateTime.Parse("2021-09-21T00:20:05", provider: ApplicationOptions.ApplicationCultureInfo),
            MlsNumber = "1531367",
            PropertyDescription = "Move in ready property on large corner lot with fenced back yard.  NEW Central air and heat, NEW Roof, NEW Water Heater and the main bathroom has been recently updated with a NEW bathtub/shower unit and vanity. Lots of space to make it a dreamy home for your family and it is large enough to run a boarding home, child care center, multi room tutoring site, business office, beauty salon or cafe. Lots of options in an approximately 3000 sq ft space with multi use zoning. Large storage building in backyard and circular drive entry and front entry ramp.",
            SellingAgent = null,
            SellingOfficeId = null,
            SpecialtyRooms = null,
            SoldPrice = null,
            SquareFeet = 2650,
            State = "TX",
            StatusDate = DateTime.Parse("2021-11-10T16:18:23", provider: ApplicationOptions.ApplicationCultureInfo),
            Status = "AO",
            ResidentialUpdateDate = DateTime.Parse("2021-11-10T16:18:23", provider: ApplicationOptions.ApplicationCultureInfo),
            ZipCode = "78861",
            Latitude = null,
            Lot = "6 7",
            Longitude = null,
            LotSize = ".3857",
            LotDimensions = "120X140",
            MiddleSchool = "Holland",
            Stories = "1",
            GarageSpaces = null,
            RecentRehab = null,
            Style = "RANCH",
            SourceSqft = "D",
            Subdivision = "HONDO",
            TaxYear = "2021",
            TotalTax = 5263,
            UnitNumber = null,
            ConstructionStartYear = "2021",
        };

        public static RoomMessage GetRoomMessage() => new()
        {
            ResidentialId = Guid.NewGuid(),
            Level = Faker.RandomNumber.Next(1, 10).ToString(),
            Type = RoomType.MasterBedroom.ToString(),
            EntityOwnerType = "SaleProperty",
        };

        public static ListingSaleStatusFieldsDto GetListingSaleStatusFieldsDto() => new()
        {
            AgentId = null,
            BackOnMarketDate = null,
            CancelDate = null,
            CancelledOption = null,
            CancelledReason = null,
            ClosedDate = null,
            ClosePrice = null,
            ContingencyInfo = null,
        };

        public static SalePropertyDetailDto GetSalePropertyDtoObject() => new()
        {
            AddressInfo = new Mock<AddressDto>().SetupAllProperties().Object,
            PropertyInfo = new Mock<PropertyDto>().SetupAllProperties().Object,
            FeaturesInfo = new Mock<FeaturesDto>().SetupAllProperties().Object,
            FinancialInfo = new Mock<FinancialDto>().SetupAllProperties().Object,
            SchoolsInfo = new Mock<SchoolsDto>().SetupAllProperties().Object,
            SalePropertyInfo = new() { OwnerName = "Kindred Homes" },
            ShowingInfo = new Mock<ShowingDto>().SetupAllProperties().Object,
            SpacesDimensionsInfo = new Mock<SpacesDimensionsDto>().SetupAllProperties().Object,
        };

        public static FullListingSaleDto GetFullListingSaleDto() => new()
        {
            CDOM = Faker.RandomNumber.Next(1, 999),
            DOM = Faker.RandomNumber.Next(1, 999),
            ExpirationDate = null,
            ListDate = DateTime.UtcNow,
            ListPrice = Faker.RandomNumber.Next(200000, 400000),
            ListType = ListType.Residential,
            MarketModifiedOn = DateTime.UtcNow,
            MlsStatus = MarketStatuses.Active,
            MlsNumber = Faker.RandomNumber.Next(10000, 20000).ToString(),
            StatusFieldsInfo = GetListingSaleStatusFieldsDto(),
            SaleProperty = GetSalePropertyDtoObject(),
        };

        public static HoaDto GetHoaDto() => new()
        {
            Name = Faker.Name.First(),
            BillingFrequency = Faker.Enum.Random<BillingFrequency>(),
            Fee = Faker.RandomNumber.Next(),
            TransferFee = Faker.RandomNumber.Next(),
        };

        public static IEnumerable<HoaDto> GetHoasDtoList(int? totalElements = 3)
        {
            var data = new List<HoaDto>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetHoaDto());
            }

            return data;
        }

        public static RoomDto GetRoomDto() => new()
        {
            RoomType = Faker.Enum.Random<RoomType>(),
            Level = Faker.Enum.Random<RoomLevel>(),
        };

        public static IEnumerable<RoomDto> GetRoomsDtoList(int? totalElements = 3)
        {
            var data = new List<RoomDto>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetRoomDto());
            }

            return data;
        }

        public static ListingSaleRequestDto GetListingSaleRequestDto() => new()
        {
            ExpirationDate = DateTime.UtcNow,
            ListDate = DateTime.UtcNow,
            ListPrice = Faker.RandomNumber.Next(200000, 400000),
            ListType = ListType.Residential,
            MarketModifiedOn = DateTime.UtcNow,
            MlsStatus = MarketStatuses.Active,
            MlsNumber = Faker.RandomNumber.Next(10000, 20000).ToString(),
            StatusFieldsInfo = GetListingSaleStatusFieldsDto(),
            SaleProperty = GetSalePropertyDtoObject(),
        };

        public static AgentValueObject GetAgentValueObject(string marketUniqueId) => new()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            CellPhone = Faker.Phone.Number(),
            Email = Faker.Internet.Email(),
            OfficeId = Faker.Name.Middle(),
            Status = "Active",
            MarketModified = DateTime.UtcNow.AddMinutes(-5),
            MarketUniqueId = marketUniqueId ?? Faker.RandomNumber.Next(10000, 90000).ToString(),
        };

        public static Agent GetAgentEntity(string marketUniqueId = null) => new(GetAgentValueObject(marketUniqueId));

        public static AgentDto GetAgentDto() => new()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            CellPhone = Faker.Phone.Number(),
            Email = Faker.Internet.Email(),
            OfficeId = Faker.Name.Middle(),
            Status = "Active",
            MarketModified = DateTime.UtcNow,
        };

        public static TrackingReverseProspect GetReverseProspectTrack(Guid listingId, Guid companyId, Guid userId, bool hasReportDtata = false) =>
            new(listingId, userId, companyId, hasReportDtata ? "[{\"Agent\":\"Joe Corwin\", \"Email\":\"joe@joecorwin.com\", \"DateSent\":\"11/29/2021\", \"InterestLevel\":\"Interested\"}]" : null, ReverseProspectStatus.Available);

        public static OfficeValueObject GetOfficeValueObject(string marketUniqueId) => new()
        {
            MarketUniqueId = marketUniqueId ?? Faker.RandomNumber.Next(10000, 90000).ToString(),
            Name = Faker.Name.First(),
            Email = Faker.Internet.Email(),
            Address = Faker.Address.StreetAddress(),
            City = Faker.Enum.Random<Cities>(),
            State = States.Texas,
            Zip = Faker.Address.ZipCode()[..5],
            Phone = Faker.Phone.Number(),
            MarketModified = DateTime.UtcNow.AddMinutes(-5),
        };

        public static Office GetOfficeEntity(string marketUniqueId = null) => new(GetOfficeValueObject(marketUniqueId));

        public static OfficeDto GetOfficeDto() => new()
        {
            MarketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString(),
            Name = Faker.Name.First(),
            Email = Faker.Internet.Email(),
            Address = Faker.Address.StreetAddress(),
            City = Faker.Enum.Random<Cities>(),
            State = States.Texas,
            Zip = Faker.Address.ZipCode()[..5],
            Phone = Faker.Phone.Number(),
            MarketModified = DateTime.UtcNow,
        };

        public static OpenHouseDto GetOpenHouseDto() => new()
        {
            StartTime = TimeSpan.Parse(DateTime.UtcNow.TimeOfDay.ToString(), formatProvider: ApplicationOptions.ApplicationCultureInfo),
            EndTime = TimeSpan.Parse(DateTime.UtcNow.AddHours(4).TimeOfDay.ToString(), formatProvider: ApplicationOptions.ApplicationCultureInfo),
            Lunch = true,
            Refreshments = false,
            Type = Faker.Enum.Random<OpenHouseType>(),
        };

        public static IEnumerable<OpenHouseDto> GetOpenHouseDtoList(int? totalElements = 4)
        {
            var data = new List<OpenHouseDto>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetOpenHouseDto());
            }

            return data;
        }

        public static SaleListingOpenHouse GetListingSaleOpenHouse()
        {
            var startTime = TimeSpan.Parse(DateTime.UtcNow.TimeOfDay.ToString(), formatProvider: ApplicationOptions.ApplicationCultureInfo);
            var endTime = TimeSpan.Parse(DateTime.UtcNow.AddHours(4).TimeOfDay.ToString(), formatProvider: ApplicationOptions.ApplicationCultureInfo);
            var type = Faker.Enum.Random<OpenHouseType>();

            return new SaleListingOpenHouse(Guid.NewGuid(), type, startTime, endTime, Faker.Boolean.Random(), Faker.Boolean.Random());
        }

        public static ICollection<SaleListingOpenHouse> GetListingSaleOpenHouses(int? totalElements = 4)
        {
            var data = new List<SaleListingOpenHouse>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetListingSaleOpenHouse());
            }

            return data;
        }

        public static SaleProperty GetSalePropertyEntityWithOpenHouses(
            Guid? planId = null,
            Guid? companyId = null)
        {
            var addressInfo = GetDefaultAddressInfo();
            var propertyInfo = GetDefaultPropertyInfo();
            var communityId = Guid.NewGuid();
            var company = companyId ?? Guid.NewGuid();
            var planProfileId = planId ?? Guid.NewGuid();

            return new SaleProperty(
                addressInfo.StreetName,
                addressInfo.StreetNumber,
                addressInfo.City,
                addressInfo.State,
                addressInfo.ZipCode,
                addressInfo.County,
                propertyInfo.ConstructionCompletionDate,
                companyId: company,
                ownerName: "some-owner",
                communityId,
                planId: planProfileId)
            {
                SalesOfficeInfo = new Mock<SalesOffice>().SetupAllProperties().Object,
                AddressInfo = new Mock<AddressInfo>().SetupAllProperties().Object,
                PropertyInfo = new Mock<PropertyInfo>().SetupAllProperties().Object,
                FeaturesInfo = new Mock<FeaturesInfo>().SetupAllProperties().Object,
                FinancialInfo = new Mock<FinancialInfo>().SetupAllProperties().Object,
                SchoolsInfo = new Mock<SchoolsInfo>().SetupAllProperties().Object,
                ShowingInfo = new Mock<ShowingInfo>().SetupAllProperties().Object,
                SpacesDimensionsInfo = new Mock<SpacesDimensionsInfo>().SetupAllProperties().Object,
                OpenHouses = GetListingSaleOpenHouses(),
                Community = GetFullCommunitySaleWithStaticValues(id: communityId),
            };
        }

        public static ListingValueObject GetListingValueObject() => new()
        {
            CDOM = Faker.RandomNumber.Next(1, 10),
            DOM = Faker.RandomNumber.Next(1, 10),
            ExpirationDate = DateTime.UtcNow,
            ListDate = DateTime.UtcNow,
            ListPrice = Faker.RandomNumber.Next(200000, 400000),
            ListType = ListType.Residential,
            MarketModifiedOn = DateTime.UtcNow,
        };

        public static ListingSaleStatusFieldsInfo GetListingStatusFieldsInfo() => new Mock<ListingSaleStatusFieldsInfo>().SetupAllProperties().Object;

        public static SalePropertyValueObject GetFullSalePropertyValueObject() => new()
        {
            OwnerName = Faker.Company.Name(),
            AddressInfo = new Mock<AddressInfo>().SetupAllProperties().Object,
            PropertyInfo = new Mock<PropertyInfo>().SetupAllProperties().Object,
            FeaturesInfo = new Mock<FeaturesInfo>().SetupAllProperties().Object,
            SchoolsInfo = new Mock<SchoolsInfo>().SetupAllProperties().Object,
            ShowingInfo = new Mock<ShowingInfo>().SetupAllProperties().Object,
            SpacesDimensionsInfo = new Mock<SpacesDimensionsInfo>().SetupAllProperties().Object,
            FinancialInfo = new Mock<FinancialInfo>().SetupAllProperties().Object,
        };

        public static DomainEntities.Listing.SaleListing GetListingSaleWithOpenHouses() =>
            new(GetListingValueObject(), GetListingStatusFieldsInfo(), GetFullSalePropertyValueObject(), Guid.NewGuid()) { SaleProperty = GetSalePropertyEntityWithOpenHouses() };

        public static CommunitySale GetFullCommunitySale(Guid? id)
        {
            var communitySale = new CommunitySale(
                id ?? Guid.NewGuid(),
                Faker.Lorem.GetFirstWord(),
                Faker.Company.Name())
            {
                Financial = new Mock<CommunityFinancialInfo>().SetupAllProperties().Object,
                Property = new()
                {
                    City = Faker.Enum.Random<Cities>(),
                    County = Faker.Enum.Random<Counties>(),
                },
            };

            return communitySale;
        }

        public static CommunitySale GetFullCommunitySaleWithStaticValues(Guid? id)
        {
            var community = new CommunitySale(
                companyId: id ?? Guid.NewGuid(),
                name: Faker.Company.Name(),
                ownerName: Faker.Company.Name())
            {
                Id = id ?? Guid.NewGuid(),
                Financial = GetDefaultCommunityFinancialInfo(),
                SchoolsInfo = GetDefaultSchoolsInfo(),
                Showing = GetDefaultCommunityShowingInfo(),
                Utilities = GetDefaultUtilities(),
                SaleOffice = GetDefaultCommunitySalesOfficeInfo(),
                Property = new()
                {
                    City = Cities.Austin,
                    County = Counties.Bexar,
                },
            };
            community.Property.ZipCode = "string";
            community.Property.MlsArea = MlsArea.WE;
            return community;
        }

        public static SaleProperty GetFullSalePropertyWithStaticValues(
            Guid? id = null,
            Guid? communityId = null,
            Guid? planId = null,
            Guid? companyId = null,
            bool addSaleListings = true)
        {
            var entityId = id ?? Guid.NewGuid();
            var communityProfileId = communityId ?? Guid.NewGuid();
            var planProfileId = planId ?? Guid.NewGuid();
            var addressInfo = GetDefaultAddressInfo();
            var propertyInfo = GetDefaultPropertyInfo();
            var saleProperty = new SaleProperty(
                addressInfo.StreetName,
                addressInfo.StreetNumber,
                addressInfo.City,
                addressInfo.State,
                addressInfo.ZipCode,
                addressInfo.County,
                propertyInfo.ConstructionCompletionDate,
                companyId: companyId ?? Guid.NewGuid(),
                ownerName: "some-owner",
                communityProfileId,
                planProfileId)
            {
                Id = id ?? Guid.NewGuid(),
                SalesOfficeInfo = GetDefaultSalesOfficeInfo(),
                AddressInfo = GetDefaultAddressInfo(),
                FeaturesInfo = GetDefaultFeaturesInfo(),
                FinancialInfo = GetDefaultFinancialInfo(),
                ShowingInfo = GetDefaultShowingInfo(),
                SchoolsInfo = GetDefaultSchoolsInfo(),
                PropertyInfo = GetDefaultPropertyInfo(),
                SpacesDimensionsInfo = GetSpacesDimensionsInfo(),
                Community = GetFullCommunitySaleWithStaticValues(communityProfileId),
            };

            if (addSaleListings)
            {
                saleProperty.SaleListings = new[] { GetListingSaleEntity(entityId, createStub: false, communityId: communityProfileId) };
            }

            return saleProperty;
        }

        public static List<DomainEntities.Listing.SaleListing> GetListOfActiveListings(bool staticValues = true)
        {
            if (staticValues)
            {
                return new List<DomainEntities.Listing.SaleListing>()
                {
                    GetListingSaleWithStaticValues(Guid.NewGuid()),
                    GetListingSaleWithStaticValues(Guid.NewGuid()),
                };
            }

            return new List<DomainEntities.Listing.SaleListing>()
            {
                GetListingSaleWithOpenHouses(),
                GetListingSaleWithOpenHouses(),
            };
        }

        public static DomainEntities.Listing.SaleListing GetListingSaleWithStaticValues(Guid? id) => new(
                GetListingValueObject(),
                GetListingStatusFieldsInfo(),
                GetFullSalePropertyValueObject(),
                Guid.NewGuid())
        {
            Id = id ?? Guid.NewGuid(),
            SaleProperty = GetFullSalePropertyWithStaticValues(id: Guid.NewGuid()),
        };

        public static IEnumerable<Company> GetCompanyInfo() => new List<Company>()
        {
            new Company()
            {
                Id = Guid.NewGuid(),
                Name = "Kindred Homes",
            },
        };

        public static CommunityEmployeeQueryResult GetCommunityEmployeeQueryResult(Guid? id, Guid? userId, string title) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Email = Faker.Internet.Email(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Title = !string.IsNullOrEmpty(title) ? title : "Test",
            UserName = Faker.Internet.UserName(),
            UserId = userId ?? Guid.NewGuid(),
        };

        public static CommunityEmployee GetCommunityEmployee(Guid? id, Guid? communityId, Guid? userId)
        {
            var employeeId = id ?? Guid.NewGuid();
            var employeeCommunityId = communityId ?? Guid.NewGuid();
            var employeeUserId = userId ?? Guid.NewGuid();
            var employee = new CommunityEmployee(employeeId, employeeCommunityId, Guid.Empty);
            employee.UserId = employeeUserId;

            return employee;
        }

        public static ListingSaleBillingQueryResult GetListingSaleBillingQueryResult(Guid? id) => new()
        {
            Id = id ?? Guid.NewGuid(),
            StreetName = Faker.Address.StreetName(),
            StreetNum = "1234",
            MlsStatus = MarketStatuses.Active,
            ListDate = DateTime.UtcNow,
            MlsNumber = "123456",
            ZipCode = Faker.Address.ZipCode()[..5],
            Subdivision = Faker.Address.City(),
            SysModifiedOn = DateTime.UtcNow,
            PublishType = ActionType.NewListing,
            ListFee = (decimal?)156.00,
        };

        public static User GetUser(Guid? id) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Email = Faker.Internet.Email(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            UserName = Faker.Internet.UserName(),
            SysCreatedOn = DateTime.UtcNow,
        };

        public static AgentQueryResult GetAgentQueryResult(Guid? agentId) => new()
        {
            AgentId = Faker.Identification.SocialSecurityNumber(),
            CompanyName = Faker.Company.Name(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Id = agentId ?? Guid.NewGuid(),
        };

        public static AddressInfo GetDefaultAddressInfo() => new()
        {
            StreetNumber = "1528",
            StreetName = "MULBERRY AVE",
            City = Cities.Austin,
            State = States.Texas,
            ZipCode = "75035",
            County = Counties.Bexar,
            LotNum = "47&48",
            Block = "7",
        };

        public static PropertyInfo GetDefaultPropertyInfo() => new()
        {
            MlsArea = MlsArea.LW,
            MapscoGrid = "stri",
            ConstructionCompletionDate = new DateTime(DateTime.UtcNow.Year, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc),
        };

        public static SpacesDimensionsInfo GetSpacesDimensionsInfo() => new()
        {
            StoriesTotal = Stories.One,
            SqFtTotal = 1254,
            DiningAreasTotal = 3,
            HalfBathsTotal = 1,
            FullBathsTotal = 3,
            LivingAreasTotal = 1,
            MainLevelBedroomTotal = 1,
            OtherLevelsBedroomTotal = 1,
            SqFtSource = SqFtSource.BuilderPlans,
        };

        public static ScrapedListingQueryResult GetScrapedListingQueryResult(
            Guid? comparisonDataId,
            decimal? listPrice,
            int? price,
            string builderName = "myTestBuilder")
        {
            var nhsListPrice = listPrice ?? Faker.RandomNumber.Next(300000, 500000);
            var nhsBuilderPrice = (int)(price ?? nhsListPrice);
            var nhsComparisonDataQueryResult = new Mock<ScrapedListingQueryResult>();
            var variance = (listPrice > 0 && price > 0) ? price - listPrice : null;
            nhsComparisonDataQueryResult.SetupAllProperties();
            nhsComparisonDataQueryResult.Object.Id = comparisonDataId ?? Guid.NewGuid();
            nhsComparisonDataQueryResult.Object.OfficeName = Faker.Company.Name();
            nhsComparisonDataQueryResult.Object.BuilderName = builderName;
            nhsComparisonDataQueryResult.Object.DOM = Faker.RandomNumber.Next();
            nhsComparisonDataQueryResult.Object.UncleanBuilder = nhsComparisonDataQueryResult.Object.BuilderName;
            nhsComparisonDataQueryResult.Object.MlsNum = Faker.RandomNumber.Next(10000).ToString();
            nhsComparisonDataQueryResult.Object.ListStatus = MarketStatuses.Active;
            nhsComparisonDataQueryResult.Object.Community = Faker.Address.UkCounty();
            nhsComparisonDataQueryResult.Object.Address = Faker.Address.StreetAddress();
            nhsComparisonDataQueryResult.Object.City = Faker.Enum.Random<Cities>().ToStringFromEnumMember();
            nhsComparisonDataQueryResult.Object.ListPrice = nhsListPrice;
            nhsComparisonDataQueryResult.Object.Price = nhsBuilderPrice;
            nhsComparisonDataQueryResult.Object.ListDate = DateTime.UtcNow;
            nhsComparisonDataQueryResult.Object.Refreshed = null;
            nhsComparisonDataQueryResult.Object.Comment = SetScrapedListingComment(nhsListPrice, nhsBuilderPrice);
            nhsComparisonDataQueryResult.Object.Variance = (int?)variance;
            return nhsComparisonDataQueryResult.Object;
        }

        public static ScrapedListingRequestFilter GetScrapedListingRequestFilter(string builderName = "myTestBuilder")
        {
            var nhsComparisonDataRequestFilter = new Mock<ScrapedListingRequestFilter>();
            nhsComparisonDataRequestFilter.SetupAllProperties();
            nhsComparisonDataRequestFilter.Object.BuilderName = builderName;
            nhsComparisonDataRequestFilter.Object.SortBy = string.Empty;
            nhsComparisonDataRequestFilter.Object.Skip = 0;
            nhsComparisonDataRequestFilter.Object.Take = 50;
            return nhsComparisonDataRequestFilter.Object;
        }

        public static SchoolsInfo GetDefaultSchoolsInfo() => new()
        {
            SchoolDistrict = SchoolDistrict.Gause,
            MiddleSchool = MiddleSchool.Gateway,
            ElementarySchool = ElementarySchool.GatesvillePri,
            HighSchool = HighSchool.Canyon,
        };

        public static Utilities GetDefaultUtilities() => new()
        {
            Fireplaces = 1,
            FireplaceDescription = GetEnumCollectionRandom<FireplaceDescription>(),
            NeighborhoodAmenities = GetEnumCollectionRandom<NeighborhoodAmenities>(),
            Floors = GetEnumCollectionRandom<Flooring>(),
            ExteriorFeatures = GetEnumCollectionRandom<ExteriorFeatures>(),
            RoofDescription = GetEnumCollectionRandom<RoofDescription>(),
            Foundation = null,
            HeatSystem = null,
            CoolingSystem = GetEnumCollectionRandom<CoolingSystem>(),
            WaterSewer = GetEnumCollectionRandom<WaterSewer>(),
        };

        public static FeaturesInfo GetDefaultFeaturesInfo()
        {
            var defaultUtilities = GetDefaultUtilities();
            return new()
            {
                Fireplaces = defaultUtilities.Fireplaces,
                FireplaceDescription = defaultUtilities.FireplaceDescription,
                NeighborhoodAmenities = defaultUtilities.NeighborhoodAmenities,
                Floors = defaultUtilities.Floors,
                ExteriorFeatures = defaultUtilities.ExteriorFeatures,
                RoofDescription = defaultUtilities.RoofDescription,
                Foundation = defaultUtilities.Foundation,
                HeatSystem = defaultUtilities.HeatSystem,
                CoolingSystem = defaultUtilities.CoolingSystem,
                WaterSewer = defaultUtilities.WaterSewer,
                PropertyDescription = "A diamond in the rough. This 1935 historic home in desirable Woodlawn Terrace is full of potential with with a detached apartment/garage at back. The living room boasts a fireplace with original hardwood flooring. Two bedrooms are located downstairs with a third bedroom up. There is a total of three full bathrooms (including the apartment) with plumbing for a fourth upstairs. Generous sized kitchen allows for a variety of makeover options. Out back the one bedroom apartment sits over a two car garage with additional parking to the side.  This home is close to downtown, Woodlawn Lake and ease of access to the highway. Come see it today!",
                WindowCoverings = GetEnumCollectionRandom<WindowFeatures>(),
                HasAccessibility = false,
                HousingStyle = GetEnumCollectionRandom<HousingStyle>(),
                HasPrivatePool = false,
                PrivatePool = GetEnumCollectionRandom<PrivatePool>(),
                HomeFaces = null,
                LotImprovements = GetEnumCollectionRandom<LotImprovements>(),
                IsNewConstruction = true,
            };
        }

        public static FinancialInfo GetDefaultFinancialInfo()
        {
            var communityFinancialInfo = GetDefaultCommunityFinancialInfo();
            return new()
            {
                TaxYear = 2020,
                HasAgentBonus = false,
                HasBonusWithAmount = false,
                AgentBonusAmount = 0,
                HasBuyerIncentive = false,
                TaxRate = communityFinancialInfo.TaxRate,
                TitleCompany = communityFinancialInfo.TitleCompany,
                HOARequirement = communityFinancialInfo.HOARequirement,
                BuyersAgentCommission = communityFinancialInfo.BuyersAgentCommission,
            };
        }

        public static SalesOffice GetDefaultSalesOfficeInfo() => new()
        {
            StreetNumber = "165",
            StreetName = "MULBERRY AVE",
            SalesOfficeCity = Cities.Austin,
            StreetSuffix = "ST",
            SalesOfficeZip = "11111",
        };

        public static ShowingInfo GetDefaultShowingInfo()
        {
            var communityShowingInfo = GetDefaultCommunityShowingInfo();
            return new()
            {
                AgentPrivateRemarks = "PROPEPRTY SOLD \"AS IS\". Apartment is not included in main house square footage. Please submit offers to davidkline@cbharper.com and cc cfinley@cbharper.com. Title company is Alamo Title, Toni Altum, 950 E. Basse Rd, 78209. Please see associated documents for offer instructions and supporting paperwork.",
                OccupantPhone = communityShowingInfo.OccupantPhone,
                ContactPhone = communityShowingInfo.ContactPhone,
                ShowingInstructions = communityShowingInfo.ShowingInstructions,
                Directions = communityShowingInfo.Directions,
                LockBoxType = communityShowingInfo.LockBoxType,
                ShowingRequirements = ShowingRequirements.AgentOrOwnerPresent,
            };
        }

        public static T[] GetEnumCollectionRandom<T>()
            where T : Enum
        {
            return new T[] { Faker.Enum.Random<T>() };
        }

        private static string SetScrapedListingComment(decimal listPrice, int price)
        {
            if (listPrice == 0)
            {
                return "Not in MLS";
            }
            else if (price == 0)
            {
                return "Not in Builder website";
            }

            return string.Empty;
        }

        private static CommunitySaleOffice GetDefaultCommunitySalesOfficeInfo() => new()
        {
            StreetNumber = "165",
            StreetName = "MULBERRY AVE",
            SalesOfficeCity = Cities.Austin,
            StreetSuffix = "ST",
            SalesOfficeZip = "11111",
        };

        private static CommunityFinancialInfo GetDefaultCommunityFinancialInfo() => new()
        {
            TaxRate = decimal.Parse("7362.26"),
            TitleCompany = "Alamo Title",
            HoaIncludes = GetEnumCollectionRandom<HoaIncludes>(),
            HOARequirement = HoaRequirement.Voluntary,
            BuyersAgentCommission = 3,
        };

        private static CommunityShowingInfo GetDefaultCommunityShowingInfo() => new()
        {
            OccupantPhone = null,
            ContactPhone = "210-222-2227",
            ShowingInstructions = "Call salesperson or come to the model home at 1234 Sample Trail.",
            Directions = "Fredericksburg Rd to Mulberry - West on Mulberry.",
            LockBoxType = LockBoxType.Combo,
        };
    }
}
