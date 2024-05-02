namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record SpacesDimensionsRecord : IProvideSummary
    {
        public const string SummarySection = "Spaces and Dimensions";
        public Stories StoriesTotal { get; set; }
        public int SqFtTotal { get; set; }
        public int DiningAreasTotal { get; set; }
        public int MainLevelBedroomTotal { get; set; }
        public int OtherLevelsBedroomTotal { get; set; }
        public int HalfBathsTotal { get; set; }
        public int FullBathsTotal { get; set; }
        public int LivingAreasTotal { get; set; }

        public SpacesDimensionsRecord CloneRecord() => (SpacesDimensionsRecord)this.MemberwiseClone();

        public static SpacesDimensionsRecord CreateRecord(SpacesDimensionsInfo spacesDimensionsInfo)
        {
            if (spacesDimensionsInfo == null)
            {
                return new();
            }

            return new()
            {
                StoriesTotal = spacesDimensionsInfo.StoriesTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.StoriesTotal)),
                SqFtTotal = spacesDimensionsInfo.SqFtTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.SqFtTotal)),
                DiningAreasTotal = spacesDimensionsInfo.DiningAreasTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.DiningAreasTotal)),
                MainLevelBedroomTotal = spacesDimensionsInfo.MainLevelBedroomTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.MainLevelBedroomTotal)),
                OtherLevelsBedroomTotal = spacesDimensionsInfo.OtherLevelsBedroomTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.OtherLevelsBedroomTotal)),
                HalfBathsTotal = spacesDimensionsInfo.HalfBathsTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.HalfBathsTotal)),
                FullBathsTotal = spacesDimensionsInfo.FullBathsTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.FullBathsTotal)),
                LivingAreasTotal = spacesDimensionsInfo.LivingAreasTotal ?? throw new DomainException(nameof(spacesDimensionsInfo.LivingAreasTotal)),
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
