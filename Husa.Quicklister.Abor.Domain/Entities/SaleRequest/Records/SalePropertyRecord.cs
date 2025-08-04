namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Common;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Request;
    using ExtensionsRecord = Husa.Quicklister.Extensions.Domain.Entities.Request.Records.SalePropertyRecord;

    public record SalePropertyRecord : ExtensionsRecord, IProvideSummary, ISalePropertyRecord<CommunitySale>
    {
        public SalePropertyRecord()
            : base()
        {
            this.OpenHouses = new List<OpenHouseRecord>();
            this.Rooms = new List<RoomRecord>();
        }

        public virtual string Address { get; set; }

        [Required]
        [ValidateProperties]
        public virtual AddressRecord AddressInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual PropertyRecord PropertyInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual SpacesDimensionsRecord SpacesDimensionsInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual FeaturesRecord FeaturesInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual FinancialRecord FinancialInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual ShowingRecord ShowingInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual SchoolRecord SchoolsInfo { get; set; }

        public virtual SalesOfficeRecord SalesOfficeInfo { get; set; }

        public virtual ICollection<OpenHouseRecord> OpenHouses { get; set; }

        public virtual ICollection<RoomRecord> Rooms { get; set; }
        public void UpdateOpenHouse<T>(ICollection<T> openHouses)
            where T : OpenHouse.OpenHouse
        {
            if (openHouses != null && openHouses.Count > 0)
            {
                this.OpenHouses = openHouses.Select(OpenHouseRecord.CreateOpenHouse).ToList();
            }
        }

        public SalePropertyRecord CloneRecord()
        {
            var clonedRecord = (SalePropertyRecord)this.MemberwiseClone();
            clonedRecord.AddressInfo = this.AddressInfo.CloneRecord();
            clonedRecord.PropertyInfo = this.PropertyInfo.CloneRecord();
            clonedRecord.SpacesDimensionsInfo = this.SpacesDimensionsInfo.CloneRecord();
            clonedRecord.FeaturesInfo = this.FeaturesInfo.CloneRecord();
            clonedRecord.FinancialInfo = this.FinancialInfo.CloneRecord();
            clonedRecord.ShowingInfo = this.ShowingInfo.CloneRecord();
            clonedRecord.SchoolsInfo = this.SchoolsInfo.CloneRecord();
            clonedRecord.SalesOfficeInfo = this.SalesOfficeInfo.CloneRecord();
            clonedRecord.OpenHouses = this.OpenHouses.Select(openHouseToClone => openHouseToClone.CloneRecord()).ToList();
            clonedRecord.Rooms = this.Rooms.Select(roomToClone => roomToClone.CloneRecord()).ToList();
            return clonedRecord;
        }

        public virtual void ImportDataFromCommunitySubmit(CommunitySale communitySale)
        {
            communitySale.SchoolsInfo.CopyProperties(this.SchoolsInfo, communitySale.GetChangedProperties(nameof(communitySale.SchoolsInfo)));
            communitySale.Financial.CopyProperties(this.FinancialInfo, communitySale.GetChangedProperties(nameof(communitySale.Financial)));
            communitySale.Showing.CopyProperties(this.ShowingInfo, communitySale.GetChangedProperties(nameof(communitySale.Showing)));
            communitySale.SaleOffice.CopyProperties(this.SalesOfficeInfo, communitySale.GetChangedProperties(nameof(communitySale.SaleOffice)));

            var utilitiesChanges = communitySale.GetChangedProperties(nameof(communitySale.Utilities));
            communitySale.Utilities.CopyProperties(this.FeaturesInfo, utilitiesChanges);
            communitySale.Utilities.CopyProperties(this.SpacesDimensionsInfo, utilitiesChanges);

            var propertyChanges = communitySale.GetChangedProperties(nameof(communitySale.Property));
            communitySale.Property.CopyProperties(this.AddressInfo, propertyChanges);
            communitySale.Property.CopyProperties(this.PropertyInfo, propertyChanges);
        }

        public static SalePropertyRecord CreateRecord(SaleProperty saleProperty)
        {
            ArgumentNullException.ThrowIfNull(saleProperty);

            var addressInfo = AddressRecord.CreateRecord(saleProperty.AddressInfo);
            var propertyInfo = PropertyRecord.CreateRecord(saleProperty.PropertyInfo);
            var spacesDimensionsInfo = SpacesDimensionsRecord.CreateRecord(saleProperty.SpacesDimensionsInfo);
            var featuresInfo = FeaturesRecord.CreateRecord(saleProperty.FeaturesInfo);
            var financialInfo = FinancialRecord.CreateRecord(saleProperty.FinancialInfo);
            var showingInfo = ShowingRecord.CreateRecord(saleProperty.ShowingInfo);
            var schoolsInfo = SchoolRecord.CreateRecord(saleProperty.SchoolsInfo);
            var salesOfficeInfo = SalesOfficeRecord.CreateRecord(saleProperty.SalesOfficeInfo);

            var salePropertyRecord = new SalePropertyRecord
            {
                Id = saleProperty.Id,
                OwnerName = saleProperty.OwnerName,
                PlanId = saleProperty.PlanId,
                PlanName = saleProperty.PlanName,
                CommunityId = saleProperty.CommunityId,
                Address = saleProperty.Address,
                SysCreatedOn = saleProperty.SysCreatedOn,
                SysModifiedOn = saleProperty.SysModifiedOn,
                IsDeleted = saleProperty.IsDeleted,
                SysModifiedBy = saleProperty.SysModifiedBy,
                SysCreatedBy = saleProperty.SysCreatedBy,
                SysTimestamp = saleProperty.SysTimestamp,
                CompanyId = saleProperty.CompanyId,
                AddressInfo = addressInfo,
                PropertyInfo = propertyInfo,
                SpacesDimensionsInfo = spacesDimensionsInfo,
                FeaturesInfo = featuresInfo,
                FinancialInfo = financialInfo,
                ShowingInfo = showingInfo,
                SchoolsInfo = schoolsInfo,
                SalesOfficeInfo = salesOfficeInfo,
            };

            salePropertyRecord.UpdateRooms(saleProperty.Rooms);
            salePropertyRecord.UpdateOpenHouse(saleProperty.OpenHouses);

            return salePropertyRecord;
        }

        public void UpdateOpenHousesFromCommunitySubmit(CommunitySale community)
        {
            if (community.HasOpenHouseChangesToSubmit)
            {
                this.OpenHouses = [.. community.OpenHouses
                    .Select(oh => Records.OpenHouseRecord.CreateOpenHouse(oh))];
            }
        }

        public virtual void UpdateInformation(SalePropertyValueObject saleProperty)
        {
            ArgumentNullException.ThrowIfNull(saleProperty);

            this.OwnerName = saleProperty.OwnerName;

            this.AddressInfo = AddressRecord.CreateRecord(saleProperty.AddressInfo);
            this.PropertyInfo = PropertyRecord.CreateRecord(saleProperty.PropertyInfo);
            this.SpacesDimensionsInfo = SpacesDimensionsRecord.CreateRecord(saleProperty.SpacesDimensionsInfo);
            this.FeaturesInfo = FeaturesRecord.CreateRecord(saleProperty.FeaturesInfo);
            this.FinancialInfo = FinancialRecord.CreateRecord(saleProperty.FinancialInfo);
            this.ShowingInfo = ShowingRecord.CreateRecord(saleProperty.ShowingInfo);
            this.SchoolsInfo = SchoolRecord.CreateRecord(saleProperty.SchoolsInfo);

            this.UpdateRooms(saleProperty.Rooms);
            this.UpdateOpenHouse(saleProperty.OpenHouses);
        }

        public virtual void UpdateRooms(ICollection<ListingSaleRoom> rooms)
        {
            if (rooms != null && rooms.Count > 0)
            {
                this.Rooms = rooms
                    .Select(rooms => RoomRecord.CreateRoom(rooms))
                    .ToList();
            }
        }

        public virtual IEnumerable<SummarySection> GetSummarySections(SalePropertyRecord previousRequestPropertyEntity)
        {
            yield return this.GetSummary(previousRequestPropertyEntity);
            yield return this.PropertyInfo.GetSummary(previousRequestPropertyEntity?.PropertyInfo);
            yield return this.AddressInfo.GetSummary(previousRequestPropertyEntity?.AddressInfo);
            yield return this.FeaturesInfo.GetSummary(previousRequestPropertyEntity?.FeaturesInfo);
            yield return this.SpacesDimensionsInfo.GetSummary(previousRequestPropertyEntity?.SpacesDimensionsInfo);
            yield return this.FinancialInfo.GetSummary(previousRequestPropertyEntity?.FinancialInfo);
            yield return this.SchoolsInfo.GetSummary(previousRequestPropertyEntity?.SchoolsInfo);
            yield return this.ShowingInfo.GetSummary(previousRequestPropertyEntity?.ShowingInfo);
            yield return this.SalesOfficeInfo?.GetSummary(previousRequestPropertyEntity?.SalesOfficeInfo);
            yield return this.GetOpenHouseSummary(previousRequestPropertyEntity?.OpenHouses);
            yield return this.GetRoomSummary(previousRequestPropertyEntity?.Rooms);
        }

        protected SummarySection GetOpenHouseSummary(IEnumerable<OpenHouseRecord> oldOpenHouses)
        {
            var section = new SummarySection
            {
                Name = OpenHouseRecord.SummarySection,
                Fields = this.OpenHouses.SummaryOpenHouse(oldOpenHouses),
            };

            if (!this.ShowingInfo.EnableOpenHouses)
            {
                return null;
            }

            return section.Fields.Any() ? section : null;
        }

        private SummarySection GetRoomSummary(IEnumerable<RoomRecord> oldRooms)
        {
            var section = new SummarySection
            {
                Name = RoomRecord.SummarySection,
                Fields = this.Rooms.GetSummaryByComparer<RoomRecord, ListingRoomComparer>(oldRooms),
            };
            return section.Fields.Any() ? section : null;
        }
    }
}
