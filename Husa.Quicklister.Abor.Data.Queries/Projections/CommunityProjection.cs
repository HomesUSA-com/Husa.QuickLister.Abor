namespace Husa.Quicklister.Abor.Data.Queries.Projections
{
    using System;
    using System.Linq.Expressions;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions.Sale;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;

    public static class CommunityProjection
    {
        public static Expression<Func<CommunitySale, CommunityQueryResult>> ProjectionToCommunityQueryResult => community => new CommunityQueryResult
        {
            Id = community.Id,
            Name = community.ProfileInfo.Name,
            Builder = community.ProfileInfo.OwnerName,
            City = community.Property.City,
            Subdivision = community.Property.Subdivision,
            ZipCode = community.Property.ZipCode,
            County = community.Property.County,
            Market = MarketCode.Austin,
            SysCreatedBy = community.SysCreatedBy,
            SysCreatedOn = community.SysCreatedOn,
            SysModifiedBy = community.SysModifiedBy,
            SysModifiedOn = community.SysModifiedOn,
            Directions = community.Showing.Directions,
            BackupPhone = community.ProfileInfo.BackupPhone,
            OfficePhone = community.ProfileInfo.OfficePhone,
        };

        public static Expression<Func<CommunitySale, CommunityDetailQueryResult>> ProjectionToCommunityDetailQueryResult => community => new CommunityDetailQueryResult
        {
            Id = community.Id,
            CompanyId = community.CompanyId,
            CommunityType = community.CommunityType,
            Profile = community.ToProjectionProfile(),
            Property = community.Property.ToProjectionProperty(),
            Utilities = community.Utilities.ToProjectionUtilities(),
            FinancialSchools = community.ToProjectionFinancialSchools(),
            Showing = community.Showing.ToProjectionCommunityShowing(),
            OpenHouses = community.OpenHouses.ToProjectionOpenHouses(),
            SysCreatedBy = community.SysCreatedBy,
            SysCreatedOn = community.SysCreatedOn,
            SysModifiedBy = community.SysModifiedBy,
            SysModifiedOn = community.SysModifiedOn,
            XmlStatus = community.XmlStatus,
            JsonImportStatus = community.JsonImportStatus,
            Directions = community.Showing.Directions,
            BackupPhone = community.ProfileInfo.BackupPhone,
            OfficePhone = community.ProfileInfo.OfficePhone,
        };

        public static Expression<Func<CommunityEmployee, CommunityEmployeeQueryResult>> ProjectionToCommunityEmployeeQueryResult => communityEmployee => new CommunityEmployeeQueryResult
        {
            Id = communityEmployee.Id,
            UserId = communityEmployee.UserId,
            CommunityId = communityEmployee.CommunityId,
            CompanyId = communityEmployee.CompanyId,
        };
    }
}
