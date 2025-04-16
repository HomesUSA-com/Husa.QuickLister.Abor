namespace Husa.Quicklister.Abor.Domain.Entities.ReverseProspect
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ReverseProspect : Entity, IProvideCompany
    {
        public ReverseProspect(Guid listingId, Guid userId, Guid companyId, string reportData, ReverseProspectStatus status)
            : this()
        {
            this.ListingId = listingId;
            this.SysCreatedBy = userId;
            this.SysModifiedBy = userId;
            this.CompanyId = companyId;
            this.ReportData = reportData;
            this.Status = status;
        }

        protected ReverseProspect()
        {
            this.Status = ReverseProspectStatus.NotYetRequested;
        }

        public Guid ListingId { get; set; }

        public ReverseProspectStatus Status { get; set; }

        public string ReportData { get; set; }

        public bool HasReportData => !string.IsNullOrWhiteSpace(this.ReportData);

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.Status;
            yield return this.ReportData;
            yield return this.ListingId;
        }
    }
}
