namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Extensions.Listing;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Communities;
    using Husa.Xml.Api.Contracts.Response;
    using CompanyExtensions = Husa.CompanyServicesManager.Api.Contracts.Response;
    using ExtensionCommunity = Husa.Quicklister.Extensions.Domain.Entities.Community.Community;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Domain.Interfaces;

    public class CommunitySale :
        ExtensionCommunity,
        IEntityOpenHouse<CommunityOpenHouse>,
        ICommunityEmployee<CommunityEmployee>,
        ExtensionsInterfaces.IProvideActiveListings<SaleListing>,
        IProvideCommunityHistory<CommunityHistory>
    {
        public CommunitySale(Guid companyId, string name, string ownerName, JsonImportStatus jsonStatus)
            : this(companyId, name, ownerName)
        {
            this.JsonImportStatus = jsonStatus;
        }

        public CommunitySale(Guid companyId, string name, string ownerName)
            : this()
        {
            this.ProfileInfo = new(name, ownerName);
            this.Showing = new(ownerName);
            this.CompanyId = companyId;
        }

        protected CommunitySale()
            : base()
        {
            this.ProfileInfo = new();
            this.SaleOffice = new();
            this.Financial = new();
            this.Showing = new();
            this.Utilities = new();
            this.SchoolsInfo = new();
            this.EmailLead = new();
            this.Property = new();
            this.OpenHouses = new HashSet<CommunityOpenHouse>();
            this.SaleProperties = new HashSet<SaleProperty>();
            this.Changes = new HashSet<string>();
            this.Employees = new HashSet<CommunityEmployee>();
            this.XmlStatus = XmlStatus.NotFromXml;
        }

        public virtual ProfileInfo ProfileInfo { get; set; }

        public virtual CommunitySaleOffice SaleOffice { get; set; }

        public virtual EmailLead EmailLead { get; set; }

        public virtual CommunityFinancialInfo Financial { get; set; }

        public virtual CommunityShowingInfo Showing { get; set; }

        public virtual Property Property { get; set; }

        public virtual Utilities Utilities { get; set; }

        public virtual SchoolsInfo SchoolsInfo { get; set; }

        public virtual ICollection<CommunityOpenHouse> OpenHouses { get; set; }

        public virtual ICollection<SaleProperty> SaleProperties { get; set; }

        public virtual ICollection<CommunityEmployee> Employees { get; set; }

        public virtual ICollection<LotListing> LotListings { get; set; }

        public virtual ICollection<ShowingTimeContact> ShowingTimeContacts { get; set; }

        public override bool CanBeDeleted => !this.SaleProperties.Any(s => s.CanBeDeleted);

        public virtual Expression<Func<SaleListing, bool>> ActiveListingsInMarketExpression => listing
            => !listing.IsDeleted && listing.SaleProperty.CommunityId == this.Id && SaleListing.ActiveListingStatuses.Contains(listing.MlsStatus) && !string.IsNullOrWhiteSpace(listing.MlsNumber);

        public virtual void UpdateProperty(Property property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.UpdateChanges(nameof(this.Property), this.Property.GetDifferences(property, exclude: new[] { nameof(this.Property.ConstructionStage), nameof(this.Property.LotDimension) }));

            if (this.Property != property)
            {
                this.Property = property;
            }
        }

        public virtual void UpdateProfile(ProfileInfo profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (this.ProfileInfo != profile)
            {
                this.ProfileInfo = profile;
            }
        }

        public virtual void UpdateSalesOffice(CommunitySaleOffice salesOffice)
        {
            if (salesOffice is null)
            {
                throw new ArgumentNullException(nameof(salesOffice));
            }

            this.UpdateChanges(nameof(this.SaleOffice), this.SaleOffice.GetDifferences(salesOffice, exclude: new[] { nameof(this.SaleOffice.IsSalesOffice) }));
            if (this.SaleOffice != salesOffice)
            {
                this.SaleOffice = salesOffice;
            }
        }

        public virtual void UpdateUtilities(Utilities utilities)
        {
            if (utilities is null)
            {
                throw new ArgumentNullException(nameof(utilities));
            }

            this.UpdateChanges(nameof(this.Utilities), this.Utilities.GetDifferences(utilities));
            if (this.Utilities != utilities)
            {
                this.Utilities = utilities;
            }
        }

        public virtual void UpdateFinancial(CommunityFinancialInfo financial)
        {
            if (financial is null)
            {
                throw new ArgumentNullException(nameof(financial));
            }

            if (!financial.IsValidBuyersAgentCommissionRange())
            {
                throw new DomainException($"The range for Buyers Agent Commission is invalid for type {financial.BuyersAgentCommissionType}");
            }

            this.UpdateChanges(nameof(this.Financial), this.Financial.GetDifferences(financial));
            if (this.Financial != financial)
            {
                this.Financial = financial;
            }
        }

        public virtual void UpdateSchools(SchoolsInfo schools)
        {
            if (schools is null)
            {
                throw new ArgumentNullException(nameof(schools));
            }

            this.UpdateChanges(nameof(this.SchoolsInfo), this.SchoolsInfo.GetDifferences(schools));
            if (this.SchoolsInfo != schools)
            {
                this.SchoolsInfo = schools;
            }
        }

        public virtual void UpdateShowing(CommunityShowingInfo showing)
        {
            if (showing is null)
            {
                throw new ArgumentNullException(nameof(showing));
            }

            this.UpdateChanges(nameof(this.Showing), this.Showing.GetDifferences(showing));
            if (this.Showing != showing)
            {
                this.Showing = showing;
            }
        }

        public virtual void UpdateEmailLeads(EmailLead emailLeads)
        {
            if (emailLeads is null)
            {
                throw new ArgumentNullException(nameof(emailLeads));
            }

            if (this.EmailLead != emailLeads)
            {
                this.EmailLead = emailLeads;
            }
        }

        public void AddOpenHouses<T>(IEnumerable<T> openHouses)
            where T : OpenHouse
        {
            var filteredOpenHouses = openHouses
            .GroupBy(o => o.Type)
            .Select(group => group.Last())
            .ToList();
            foreach (var openHouseDetail in filteredOpenHouses)
            {
                var openHouse = new CommunityOpenHouse(
                    this.Id,
                    openHouseDetail.Type,
                    openHouseDetail.StartTime,
                    openHouseDetail.EndTime,
                    openHouseDetail.Refreshments);

                this.OpenHouses.Add(openHouse);
            }
        }

        public virtual void AddCommunityEmployee(Guid userId)
        {
            this.Employees.Add(new CommunityEmployee(userId, this.Id, this.CompanyId));
        }

        public virtual void AddCommunityEmployees(IEnumerable<Guid> userIds)
        {
            foreach (var userId in userIds)
            {
                this.Employees.Add(new CommunityEmployee(userId, this.Id, this.CompanyId));
            }
        }

        public virtual void DeleteCommunityEmployees(IEnumerable<Guid> userIds, IUserContext user)
        {
            foreach (var userId in userIds)
            {
                this.CanDeleteEmployee(userId, user);
                var employee = this.Employees.SingleOrDefault(e => e.UserId == userId);
                this.Employees.Remove(employee);
            }
        }

        public virtual IEnumerable<SaleListing> GetListingsToUpdate() => this.SaleProperties.GetListingsToUpdate();

        public virtual IEnumerable<LotListing> GetActiveLotListings() => this.LotListings
            .Where(listing => !listing.IsDeleted && MarketStatusesExtensions.ActiveListingStatuses.Contains(listing.MlsStatus) && !string.IsNullOrWhiteSpace(listing.MlsNumber));

        public virtual bool IsEmployee(Guid id) => this.Employees.Any(e => e.UserId == id);

        public virtual void CanUpdate(IUserContext user)
        {
            if (user.UserRole == UserRole.User && user.EmployeeRole != RoleEmployee.CompanyAdmin && !this.IsEmployee(user.Id))
            {
                throw new DomainException($"The user {user.Name} is not employee for the community {this.ProfileInfo.Name}");
            }
        }

        public virtual void CanDeleteEmployee(Guid userEmployeeId, IUserContext user)
        {
            if (user.UserRole == UserRole.User &&
                user.EmployeeRole != RoleEmployee.CompanyAdmin &&
                !this.IsEmployee(user.Id))
            {
                throw new DomainException($"The user {user.Name} is not employee for the community {this.ProfileInfo.Name}");
            }

            if (user.EmployeeRole == RoleEmployee.SalesEmployee && user.Id != userEmployeeId)
            {
                throw new DomainException($"The user {user.Name} is not allowed to execute this action");
            }
        }

        public virtual void ImportFromListing(SaleListing listing)
        {
            this.Property = this.Property.ImportProperty(listing.SaleProperty);
            this.Financial = this.Financial.ImportFinancial(listing.SaleProperty.FinancialInfo);
            this.Showing = this.Showing.ImportShowing(listing.SaleProperty.ShowingInfo);
            this.Utilities = this.Utilities.ImportUtilities(listing.SaleProperty.FeaturesInfo);
            this.SchoolsInfo = this.SchoolsInfo.ImportSchools(listing.SaleProperty.SchoolsInfo);

            this.ImportOpenHouse<SaleListingOpenHouse, CommunityOpenHouse, CommunitySale>(listing.SaleProperty.OpenHouses);
        }

        public virtual void UpdateCompanyInfo(
            Guid companyId,
            string companyName)
        {
            this.ProfileInfo.OwnerName = companyName;
            this.CompanyId = companyId;
        }

        public virtual void ProcessXmlData(SubdivisionResponse subdivision, string companyName, bool isCentralizedEmailLeads = false)
        {
            if (!subdivision.CommunityProfileId.HasValue || subdivision.CommunityProfileId == Guid.Empty)
            {
                this.ImportFromXml(subdivision, companyName, isCentralizedEmailLeads);
                return;
            }

            this.UpdateFromXml(subdivision, companyName, isCentralizedEmailLeads);
        }

        public virtual void UpdateFromXml(SubdivisionResponse subdivision, string companyName, bool isCentralizedEmailLeads = false)
        {
            var profile = this.ProfileInfo.UpdateFromXml(subdivision.SaleOffice, companyName);
            this.UpdateProfile(profile);
            var salesOffice = this.SaleOffice.UpdateFromXml(subdivision.SaleOffice);
            this.UpdateSalesOffice(salesOffice);
            var property = this.Property.UpdateFromXml(subdivision);
            this.UpdateProperty(property);
            var financialInfo = this.Financial.UpdateFromXml(subdivision);
            this.UpdateFinancial(financialInfo);
            var showingInfo = this.Showing.UpdateFromXml(subdivision);
            this.UpdateShowing(showingInfo);

            if (!isCentralizedEmailLeads)
            {
                var emailLeads = this.EmailLead.UpdateFromXml(subdivision);
                this.UpdateEmailLeads(emailLeads);
            }

            var openHouses = subdivision.OpenHouses.Select(openHouse => CommunityOpenHouse.ImportFromXml(openHouse, openHouse.Day));
            this.UpdateOpenHouse(openHouses);
        }

        public virtual void ImportFromXml(SubdivisionResponse subdivision, string companyName, bool isCentralizedEmailLeads = false)
        {
            this.XmlStatus = XmlStatus.AwaitingApproval;
            var cityEnum = subdivision.City.ToCity(isExactValue: false);
            var countyEnum = subdivision.County.ToCounty(isExactValue: false);
            this.UpdateAddressInfo(cityEnum, countyEnum);

            var profile = ProfileInfo.ImportFromXml(subdivision, companyName, this.ProfileInfo);
            this.UpdateProfile(profile);

            var salesOffice = CommunitySaleOffice.ImportFromXml(subdivision, this.SaleOffice);
            this.UpdateSalesOffice(salesOffice);

            var property = Property.ImportFromXml(subdivision, this.Property);
            this.UpdateProperty(property);

            var financialInfo = CommunityFinancialInfo.ImportFromXml(subdivision, this.Financial);
            this.UpdateFinancial(financialInfo);

            var schoolInfo = SchoolsInfo.ImportFromXml(subdivision, this.SchoolsInfo);
            this.UpdateSchools(schoolInfo);

            var showingInfo = CommunityShowingInfo.ImportFromXml(subdivision, this.Showing);
            this.UpdateShowing(showingInfo);

            var utilitiesInfo = Utilities.ImportFromXml(subdivision, this.Utilities);
            this.UpdateUtilities(utilitiesInfo);

            if (!isCentralizedEmailLeads)
            {
                var emailLeads = EmailLead.ImportFromXml(subdivision, this.EmailLead);
                this.UpdateEmailLeads(emailLeads);
            }

            var openHouses = subdivision.OpenHouses.Select(openHouse => CommunityOpenHouse.ImportFromXml(openHouse, openHouse.Day));
            this.UpdateOpenHouse(openHouses);
        }

        public virtual void UpdateAddressInfo(Cities? city = null, Counties? county = null)
        {
            if (city.HasValue)
            {
                this.Property.City = city.Value;
            }

            if (county.HasValue)
            {
                this.Property.County = county.Value;
            }
        }

        public virtual void UpdateCompanyEmailLeads(IEnumerable<CompanyExtensions.EmailLead> emailLeads)
        {
            if (emailLeads is null || !emailLeads.Any())
            {
                return;
            }

            this.EmailLead.EmailLeadPrincipal = emailLeads.Where(x => x.EmailPriority == EmailPriority.One && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
            this.EmailLead.EmailLeadSecondary = emailLeads.Where(x => x.EmailPriority == EmailPriority.Two && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
            this.EmailLead.EmailLeadThird = emailLeads.Where(x => x.EmailPriority == EmailPriority.Three && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
            this.EmailLead.EmailLeadFourth = emailLeads.Where(x => x.EmailPriority == EmailPriority.Four && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
            this.EmailLead.EmailLeadFifth = emailLeads.Where(x => x.EmailPriority == EmailPriority.Five && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
            this.EmailLead.EmailLeadOther = emailLeads.Where(x => x.EmailPriority == EmailPriority.Six && x.EntityType == EmailEntityType.Sale).Select(x => x.Email).FirstOrDefault();
        }

        public virtual void Update(
            CommunityValueObject communityInfo,
            IEnumerable<CommunityOpenHouse> communityOpenHouses)
        {
            if (communityInfo is null)
            {
                throw new ArgumentNullException(nameof(communityInfo));
            }

            this.UpdateProperty(property: communityInfo.PropertyInfo);
            this.UpdateProfile(profile: communityInfo.ProfileInfo);
            this.UpdateSalesOffice(salesOffice: communityInfo.SalesOfficeInfo);
            this.UpdateEmailLeads(emailLeads: communityInfo.EmailLeadInfo);
            this.UpdateUtilities(utilities: communityInfo.UtilitiesInfo);
            this.UpdateFinancial(financial: communityInfo.FinancialInfo);
            this.UpdateSchools(schools: communityInfo.SchoolsInfo);
            this.UpdateShowing(showing: communityInfo.ShowingInfo);
            this.UpdateOpenHouse(openHouses: communityOpenHouses);
            this.UpdateShowingTime(communityInfo.UseShowingTime, communityInfo.ShowingTime);
        }

        public CommunityHistory GenerateRecord()
        {
            var record = new CommunityHistory(this.Id)
            {
                ProfileInfo = this.ProfileInfo.Clone(),
                EmailLead = this.EmailLead.Clone(),
                Financial = this.Financial.Clone(),
                Showing = this.Showing.Clone(),
                Property = this.Property.Clone(),
                Utilities = this.Utilities.Clone(),
                SchoolsInfo = this.SchoolsInfo.Clone(),
                SaleOffice = this.SaleOffice,
            };
            record.ImportOpenHouse<CommunityOpenHouse, OpenHouse, CommunityHistory>(this.OpenHouses);
            return record;
        }

        protected override void DeleteChildren(Guid userId)
        {
            foreach (var listing in this.SaleProperties)
            {
                listing.Delete(userId);

                listing.CommunityId = null;

                listing.UpdateTrackValues(userId);
            }
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.ProfileInfo;
            yield return this.SaleOffice;
            yield return this.EmailLead;
            yield return this.Financial;
            yield return this.Showing;
            yield return this.Property;
            yield return this.Utilities;
            yield return this.SchoolsInfo;
            yield return this.AccessInformation;
            yield return this.AdditionalInstructions;
            yield return this.AppointmentRestrictions;
        }
    }
}
