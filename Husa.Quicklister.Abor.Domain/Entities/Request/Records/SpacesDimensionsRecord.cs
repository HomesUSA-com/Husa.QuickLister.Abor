namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record SpacesDimensionsRecord : IProvideSummary
    {
        public const string SummarySection = "Spaces and Dimensions";
        public CategoryType TypeCategory { get; set; }

        [Required]
        public int? SqFtTotal { get; set; }

        public SqFtSource? SqFtSource { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }

        public ICollection<OtherParking> OtherParking { get; set; }

        [Required]
        public Stories Stories { get; set; }

        [Required]
        public int? BathsFull { get; set; }

        [Required]
        public int? BathsHalf { get; set; }

        [Required]
        public int? NumBedrooms { get; set; }

        [Required]
        public ICollection<GarageDescription> GarageDescription { get; set; }

        public SpacesDimensionsRecord CloneRecord() => (SpacesDimensionsRecord)this.MemberwiseClone();

        public static SpacesDimensionsRecord CreateRecord(SpacesDimensionsInfo spacesDimensionsInfo)
        {
            if (spacesDimensionsInfo == null)
            {
                return new();
            }

            return new()
            {
                TypeCategory = spacesDimensionsInfo.TypeCategory,
                SqFtTotal = spacesDimensionsInfo.SqFtTotal,
                SqFtSource = spacesDimensionsInfo.SqFtSource,
                SpecialtyRooms = spacesDimensionsInfo.SpecialtyRooms,
                OtherParking = spacesDimensionsInfo.OtherParking,
                Stories = spacesDimensionsInfo.Stories ?? throw new DomainException(nameof(spacesDimensionsInfo.Stories)),
                BathsFull = spacesDimensionsInfo.BathsFull,
                BathsHalf = spacesDimensionsInfo.BathsHalf,
                NumBedrooms = spacesDimensionsInfo.NumBedrooms,
                GarageDescription = spacesDimensionsInfo.GarageDescription,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
