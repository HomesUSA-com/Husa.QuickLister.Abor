namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public class CommunitySaleResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public CommunityType CommunityType { get; set; }
        public CommunityPropertyResponse Property { get; set; }
        public CommunityProfileResponse Profile { get; set; }
        public FeaturesAndUtilitiesResponse Utilities { get; set; }
        public CommunityFinancialResponse FinancialSchools { get; set; }
        public CommunityShowingResponse Showing { get; set; }
        public IEnumerable<OpenHouseResponse> OpenHouses { get; set; }
        public DateTime? SysModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public Guid? SysCreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Guid? SysModifiedBy { get; set; }
        public DateTime SysCreatedOn { get; set; }
        public DateTime? LastPhotoRequestCreationDate { get; set; }
        public Guid? LastPhotoRequestId { get; set; }
        public XmlStatus XmlStatus { get; set; }
        public string Directions { get; set; }
        public string OfficePhone { get; set; }
        public string BackupPhone { get; set; }
        public string ZipCode { get; set; }
        public string Name { get; set; }
        public Cities? City { get; set; }
    }
}
