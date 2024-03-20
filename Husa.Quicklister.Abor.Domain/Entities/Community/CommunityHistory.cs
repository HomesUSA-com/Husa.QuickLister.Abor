namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
    using ExtensionsCommunity = Husa.Quicklister.Extensions.Domain.Entities.Community.CommunityHistory;

    public class CommunityHistory : ExtensionsCommunity, IEntityOpenHouse<OpenHouse>
    {
        public CommunityHistory()
            : base()
        {
            this.OpenHouses = new List<OpenHouse>();
        }

        public CommunityHistory(Guid communityId)
            : base(communityId)
        {
            this.OpenHouses = new List<OpenHouse>();
        }

        public virtual ProfileInfo ProfileInfo { get; set; }
        public virtual CommunitySaleOffice SaleOffice { get; set; }
        public virtual EmailLead EmailLead { get; set; }
        public virtual CommunityFinancialInfo Financial { get; set; }
        public virtual CommunityShowingInfo Showing { get; set; }
        public virtual Property Property { get; set; }
        public virtual Utilities Utilities { get; set; }
        public virtual SchoolsInfo SchoolsInfo { get; set; }
        public virtual ICollection<OpenHouse> OpenHouses { get; set; }

        public virtual void AddOpenHouses<T>(IEnumerable<T> openHouses)
            where T : OpenHouse
        {
            var filteredOpenHouses = openHouses
            .GroupBy(o => o.Type)
            .Select(group => group.Last())
            .ToList();
            foreach (var openHouse in filteredOpenHouses)
            {
                this.OpenHouses.Add(new(
                    openHouse.Type,
                    startTime: openHouse.StartTime,
                    endTime: openHouse.EndTime,
                    refreshments: openHouse.Refreshments));
            }
        }

        public IEnumerable<SummarySection> GetSummary(CommunityHistory previousCommunity)
        {
            if (previousCommunity == null)
            {
                return Array.Empty<SummarySection>();
            }

            var summarySections = new List<SummarySection>()
            {
                this.GetRootSummary(previousCommunity),
                this.ProfileInfo.GetSummarySection(previousCommunity.ProfileInfo, nameof(this.ProfileInfo)),
                this.SaleOffice.GetSummarySection(previousCommunity.SaleOffice, nameof(this.SaleOffice)),
                this.EmailLead.GetSummarySection(previousCommunity.EmailLead, nameof(this.EmailLead)),
                this.Financial.GetSummarySection(previousCommunity.Financial, nameof(this.Financial)),
                this.Showing.GetSummarySection(previousCommunity.Showing, nameof(this.Showing)),
                this.Property.GetSummarySection(previousCommunity.Property, nameof(this.Property)),
                this.Utilities.GetSummarySection(previousCommunity.Utilities, nameof(this.Utilities)),
                this.SchoolsInfo.GetSummarySection(previousCommunity.SchoolsInfo, nameof(this.SchoolsInfo)),
            };

            var openHouseSummaryFields = this.OpenHouses.GetSummaryByComparer<OpenHouse, OpenHouseComparer>(previousCommunity.OpenHouses);
            if (openHouseSummaryFields.Any())
            {
                summarySections.Add(new()
                {
                    Name = nameof(this.OpenHouses),
                    Fields = openHouseSummaryFields,
                });
            }

            return summarySections.Where(section => section != null);
        }

        public override IEnumerable<SummarySection> GetSummary<TRecord>(TRecord previousRecord)
            => this.GetSummary(previousRecord as CommunityHistory);
    }
}
