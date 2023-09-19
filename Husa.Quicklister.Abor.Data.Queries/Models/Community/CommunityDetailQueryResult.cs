namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class CommunityDetailQueryResult : BaseQueryResult
    {
        public Guid CompanyId { get; set; }
        public CommunityType CommunityType { get; set; }
        public FinancialSchoolsQueryResult FinancialSchools { get; set; }
        public ShowingQueryResult Showing { get; set; }
        public ProfileQueryResult Profile { get; set; }
        public PropertyQueryResult Property { get; set; }
        public UtilitiesQueryResult Utilities { get; set; }
        public DateTime? LastPhotoRequestCreationDate { get; set; }
        public Guid? LastPhotoRequestId { get; set; }
        public IEnumerable<OpenHousesQueryResult> OpenHouses { get; set; }
        public XmlStatus XmlStatus { get; set; }
    }
}
