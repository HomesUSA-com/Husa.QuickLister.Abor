namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record SpacesDimensionsRecord : IProvideSummary, IProvideSpacesDimensions
    {
        public const string SummarySection = "Spaces and Dimensions";

        [Required]
        public Stories? StoriesTotal { get; set; }

        [Required]
        public int? SqFtTotal { get; set; }
        [Required]
        public int? DiningAreasTotal { get; set; }
        [Required]
        public int? MainLevelBedroomTotal { get; set; }

        [Required]
        [Range(0, 9, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? OtherLevelsBedroomTotal { get; set; }
        [Required]
        public int? HalfBathsTotal { get; set; }
        [Required]
        public int? FullBathsTotal { get; set; }
        [Required]
        public int? LivingAreasTotal { get; set; }

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
                OtherLevelsBedroomTotal = spacesDimensionsInfo.OtherLevelsBedroomTotal,
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
